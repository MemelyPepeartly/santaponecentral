using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Api.Models.Message_Models;
using Santa.Logic.Interfaces;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IRepository repository;
        public MessageController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));

        }
        // GET: api/Message
        /// <summary>
        /// Gets all messages sent in DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Logic.Objects.Message>> GetAllMessages()
        {
            try
            {
                return Ok(repository.GetAllMessages());
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
                    messageRecieverClientID = message.messageRecieverClientID == null ? null : message.messageRecieverClientID,
                    messageSenderClientID = message.messageSenderClientID == null ? null : message.messageSenderClientID,
                    clientRelationXrefID = message.clientRelationXrefID == null ? null : message.clientRelationXrefID,
                    messageContent = message.messageContent,
                    isMessageRead = false,
                };
                if (logicMessage.messageRecieverClientID == null && logicMessage.messageSenderClientID == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                else
                {
                    await repository.CreateMessage(logicMessage);
                    await repository.SaveAsync();
                    return Ok(await repository.GetMessageByIDAsync(logicMessage.chatMessageID));
                }
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
