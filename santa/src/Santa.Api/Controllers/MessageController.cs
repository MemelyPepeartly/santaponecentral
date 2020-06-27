using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.AspNetCore.SignalR;
using Santa.Api.Models.Message_Models;
using Santa.Logic.Interfaces;
using SignalRChat.Hubs;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IRepository repository;
        private IHubContext<ChatHub> hub;
        public MessageController(IRepository _repository, IHubContext<ChatHub> _hub)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            hub = _hub ?? throw new ArgumentNullException(nameof(_hub));

        }
        // GET: api/Message
        /// <summary>
        /// Gets all messages sent in DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Logic.Objects.Message>>> GetAllMessages()
        {
            try
            {
                return Ok(await repository.GetAllMessages());
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // GET: api/Message/5
        /// <summary>
        /// Gets certain message by ID
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        [HttpGet("{chatMessageID}", Name = "Get")]
        public async Task<ActionResult<Logic.Objects.Message>> GetMessage(Guid chatMessageID)
        {
            try
            {
                return Ok(await repository.GetMessageByIDAsync(chatMessageID));
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        // POST: api/Message
        [HttpPost]
        public async Task<ActionResult<Logic.Objects.Message>> PostMessage([FromBody, Bind("messageSenderClientID, messageRecieverClientID, messageContent, clientRelationXrefID")] ApiMessage message)
        {
            try
            {
                Logic.Objects.Message logicMessage = new Logic.Objects.Message()
                {
                    chatMessageID = Guid.NewGuid(),
                    recieverClient = new Logic.Objects.ClientMeta()
                    {
                        clientId = message.messageRecieverClientID
                    },
                    senderClient = new Logic.Objects.ClientMeta()
                    {
                        clientId = message.messageSenderClientID
                    },
                    clientRelationXrefID = message.clientRelationXrefID,
                    messageContent = message.messageContent,
                    dateTimeSent = DateTime.UtcNow,
                    isMessageRead = false,
                };
                if (logicMessage.recieverClient.clientId == null && logicMessage.senderClient.clientId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                else
                {
                    await repository.CreateMessage(logicMessage);
                    await repository.SaveAsync();
                    return Ok();
                }
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // PUT: api/Message/5
        [HttpPut("{chatMessageID}/Read")]
        public async Task<ActionResult<Logic.Objects.Message>> PutDescription(Guid chatMessageID, [FromBody, Bind("isMessageRead")] ApiMessageRead message)
        {
            try
            {
                Logic.Objects.Message targetMessage = await repository.GetMessageByIDAsync(chatMessageID);
                targetMessage.isMessageRead = message.isMessageRead;
                try
                {
                    await repository.UpdateMessageByIDAsync(targetMessage);
                    await repository.SaveAsync();
                    return Ok(await repository.GetMessageByIDAsync(chatMessageID));
                }
                catch (Exception e)
                {
                    throw e.InnerException;
                }
            }

            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
