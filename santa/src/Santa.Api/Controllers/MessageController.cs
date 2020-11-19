using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Santa.Api.Models.Message_Models;
using Santa.Api.SendGrid;
using Santa.Api.Services.YuleLog;
using Santa.Logic.Constants;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects.Information_Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IMailbag mailbag;
        private readonly IYuleLog yuleLogger;

        public MessageController(IRepository _repository, IMailbag _mailbag, IYuleLog _yuleLogger)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            mailbag = _mailbag ?? throw new ArgumentNullException(nameof(_mailbag));
            yuleLogger = _yuleLogger ?? throw new ArgumentNullException(nameof(_yuleLogger));
        }
        // GET: api/Message
        /// <summary>
        /// Gets all messages sent in DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "read:messages")]
        public async Task<ActionResult<List<Logic.Objects.Message>>> GetAllMessages()
        {
            return Ok(await repository.GetAllMessages());
        }

        // GET: api/Message/5
        /// <summary>
        /// Gets certain message by ID
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        [HttpGet("{chatMessageID}", Name = "Get")]
        [Authorize(Policy = "read:messages")]
        public async Task<ActionResult<Logic.Objects.Message>> GetMessage(Guid chatMessageID)
        {
            return Ok(await repository.GetMessageByIDAsync(chatMessageID));
        }

        // POST: api/Message
        [HttpPost]
        [Authorize(Policy = "create:messages")]
        public async Task<ActionResult<Logic.Objects.Message>> PostMessage([FromBody] ApiMessageModel message)
        {
            Logic.Objects.Client logicClient = await repository.GetClientByIDAsync(message.messageSenderClientID.GetValueOrDefault() != Guid.Empty ? message.messageSenderClientID.GetValueOrDefault() : message.messageRecieverClientID.GetValueOrDefault());
            Logic.Objects.Client checkerClient = await repository.GetClientByEmailAsync(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            BaseClient baseCheckerClient = new BaseClient()
            {
                clientID = checkerClient.clientID,
                clientName = checkerClient.clientName,
                nickname = checkerClient.nickname,
                email = checkerClient.email,
                hasAccount = checkerClient.hasAccount,
                isAdmin = checkerClient.isAdmin
            };

            // If the logic client and checker client have the same Id and the relationxref is either null or part of of the checked clients assingments list (Or the checker is just an admin overall)
            if ((logicClient.clientID == checkerClient.clientID && message.clientRelationXrefID != null ? checkerClient.assignments.Any(a => a.clientRelationXrefID == message.clientRelationXrefID) : true) || checkerClient.isAdmin)
            {
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                Logic.Objects.Message logicMessage = new Logic.Objects.Message()
                {
                    chatMessageID = Guid.NewGuid(),
                    recieverClient = new ClientChatMeta()
                    {
                        clientId = message.messageRecieverClientID
                    },
                    senderClient = new ClientChatMeta()
                    {
                        clientId = message.messageSenderClientID
                    },
                    clientRelationXrefID = message.clientRelationXrefID,
                    messageContent = message.messageContent,
                    dateTimeSent = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone),
                    isMessageRead = false,
                    fromAdmin = message.fromAdmin
                };
                if (logicMessage.recieverClient.clientId == null && logicMessage.senderClient.clientId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                else
                {
                    try
                    {
                        await repository.CreateMessage(logicMessage);
                        await repository.SaveAsync();

                        Logic.Objects.Message postedLogicMessage = await repository.GetMessageByIDAsync(logicMessage.chatMessageID);
                        await yuleLogger.logCreatedNewMessage(baseCheckerClient, postedLogicMessage.senderClient, postedLogicMessage.recieverClient);

                        // If this message has an eventTypeID
                        if (message.eventTypeID.HasValue)
                        {
                            // If the message is from an admin, get the event for the notification, and send the email
                            if (message.fromAdmin)
                            {
                                Logic.Objects.Event logicEvent = await repository.GetEventByIDAsync(message.eventTypeID.Value);
                                await mailbag.sendChatNotificationEmail(await repository.GetClientByIDAsync(logicMessage.recieverClient.clientId.Value), logicEvent);
                            }
                        }
                        // Else if it doesnt have an event (It is a general message)
                        else
                        {
                            // If it's from an admin, make a new event object, and send the client a notification
                            if (message.fromAdmin)
                            {
                                Logic.Objects.Event logicEvent = new Logic.Objects.Event();
                                await mailbag.sendChatNotificationEmail(await repository.GetClientByIDAsync(logicMessage.recieverClient.clientId.Value), new Logic.Objects.Event());
                            }
                        }
                        return Ok();
                    }
                    catch(Exception)
                    {
                        await yuleLogger.logError(baseCheckerClient, LoggingConstants.CREATED_NEW_MESSAGE_CATEGORY);
                        return StatusCode(StatusCodes.Status424FailedDependency);
                    }
                    
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

        }

        // PUT: api/Message/5
        [HttpPut("{chatMessageID}/Read")]
        [Authorize(Policy = "update:messages")]
        public async Task<ActionResult<Logic.Objects.Message>> PutReadStatus(Guid chatMessageID, [FromBody] ApiMessageReadModel message)
        {
            Logic.Objects.Message logicMessage = await repository.GetMessageByIDAsync(chatMessageID);
            BaseClient checkerClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            if(checkerClient.isAdmin || logicMessage.recieverClient.clientId == checkerClient.clientID)
            {
                try
                {
                    Logic.Objects.Message targetMessage = await repository.GetMessageByIDAsync(chatMessageID);
                    targetMessage.isMessageRead = message.isMessageRead;

                    await repository.UpdateMessageByIDAsync(targetMessage);
                    await repository.SaveAsync();
                    Logic.Objects.Message modifiedLogicMessage = await repository.GetMessageByIDAsync(chatMessageID);
                    await yuleLogger.logModifiedMessageReadStatus(checkerClient, modifiedLogicMessage);
                    return Ok(modifiedLogicMessage);
                }
                catch (Exception)
                {
                    await yuleLogger.logError(checkerClient, LoggingConstants.MODIFIED_MESSAGE_READ_STATUS_CATEGORY);
                    return StatusCode(StatusCodes.Status424FailedDependency);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
        }

        // PUT: api/Message/ReadAll
        [HttpPut("ReadAll")]
        [Authorize(Policy = "update:messages")]
        public async Task<ActionResult<Logic.Objects.Message>> PutReadAll([FromBody] ApiReadAllMessageModel messages)
        {
            Logic.Objects.Client checkerClient = await repository.GetClientByEmailAsync(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            foreach (Guid messageID in messages.messages)
            {
                Logic.Objects.Message targetMessage = await repository.GetMessageByIDAsync(messageID);
                if (checkerClient.isAdmin ||  targetMessage.recieverClient.clientId == checkerClient.clientID)
                {
                    targetMessage.isMessageRead = true;
                    await repository.UpdateMessageByIDAsync(targetMessage);
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
            }

            await repository.SaveAsync();
            return Ok();
        }
    }
}
