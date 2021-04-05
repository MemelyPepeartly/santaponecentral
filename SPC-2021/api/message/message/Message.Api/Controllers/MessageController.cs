using Message.Api.Models.Message_Models;
using Message.Logic.Interfaces;
using Message.Logic.Objects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Message.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageController : ControllerBase
    {
#warning All mailbag and YuleLog functionality will need to be updated from legacy to support microservice infrastructure
        private readonly IRepository repository;
        //private readonly IMailbag mailbag;
        //private readonly IYuleLog yuleLogger;

        public MessageController(IRepository _repository/*, IMailbag _mailbag, IYuleLog _yuleLogger*/)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            //mailbag = _mailbag ?? throw new ArgumentNullException(nameof(_mailbag));
            //yuleLogger = _yuleLogger ?? throw new ArgumentNullException(nameof(_yuleLogger));
        }
        // GET: api/Message
        /// <summary>
        /// Gets all messages sent in DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "read:messages")]
        public async Task<ActionResult<List<ChatMessage>>> GetAllMessages()
        {
            return Ok(await repository.GetAllMessages());
        }

        // POST: api/Message
        [HttpPost]
        [Authorize(Policy = "create:messages")]
        public async Task<ActionResult<ChatMessage>> PostMessage([FromBody] ApiMessageModel message)
        {
            /*
            BaseClient logicBaseClient = await repository.GetBasicClientInformationByID(message.messageSenderClientID.GetValueOrDefault() != Guid.Empty ? message.messageSenderClientID.GetValueOrDefault() : message.messageRecieverClientID.GetValueOrDefault());
            BaseClient checkerClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            List<RelationshipMeta> checkerAssignmentInfo = new List<RelationshipMeta>();
            // If the checker is an admin, there is no need to get the checker's info container as that is
            // only used to authorize the caller
            if (!checkerClient.isAdmin)
            {
                checkerAssignmentInfo = await repository.getClientAssignmentsInfoByIDAsync(checkerClient.clientID);
            }


            // If the logic client and checker client have the same Id
            if (logicBaseClient.clientID == checkerClient.clientID)
            {
                // If the message xref is not null, check to make sure the checkerAssignmentInfo has an assignment that matches (Or pass true if null, which implies posting to a general chat
                // Or bypass if they are an admin
                if ((message.clientRelationXrefID != null ? checkerAssignmentInfo.Any(a => a.clientRelationXrefID == message.clientRelationXrefID) : true) || checkerClient.isAdmin)
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
                            await yuleLogger.logCreatedNewMessage(checkerClient, postedLogicMessage.senderClient, postedLogicMessage.recieverClient);

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
                        catch (Exception)
                        {
                            await yuleLogger.logError(checkerClient, LoggingConstants.CREATED_NEW_MESSAGE_CATEGORY);
                            return StatusCode(StatusCodes.Status424FailedDependency);
                        }

                    }
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized);
            */
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // PUT: api/Message/5
        [HttpPut("{chatMessageID}/Read")]
        [Authorize(Policy = "update:messages")]
        public async Task<ActionResult<ChatMessage>> PutReadStatus(Guid chatMessageID, [FromBody] ApiMessageReadModel message)
        {
            /*
            Message logicMessage = await repository.GetMessageByIDAsync(chatMessageID);
            BaseClient checkerClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            if (checkerClient.isAdmin || logicMessage.recieverClient.clientId == checkerClient.clientID)
            {
                try
                {
                    Message targetMessage = await repository.GetMessageByIDAsync(chatMessageID);
                    targetMessage.isMessageRead = message.isMessageRead;

                    await repository.UpdateMessageByIDAsync(targetMessage);
                    await repository.SaveAsync();
                    Message modifiedLogicMessage = await repository.GetMessageByIDAsync(chatMessageID);
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
            */
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // PUT: api/Message/ReadAll
        [HttpPut("ReadAll")]
        [Authorize(Policy = "update:messages")]
        public async Task<ActionResult<ChatMessage>> PutReadAll([FromBody] ApiReadAllMessageModel messages)
        {
            /*
            Client checkerClient = await repository.GetClientByEmailAsync(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            foreach (Guid messageID in messages.messages)
            {
                ChatMessage targetMessage = await repository.GetMessageByIDAsync(messageID);
                if (checkerClient.isAdmin || targetMessage.recieverClient.clientId == checkerClient.clientID)
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
            */
            return StatusCode(StatusCodes.Status501NotImplemented);

        }
    }
}
