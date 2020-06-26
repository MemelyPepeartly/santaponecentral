using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IRepository repository;
        public HistoryController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));

        }

        // GET: api/History/Client/5/Relationship/5
        /// <summary>
        /// Gets a specific history by clientID and an optional relationship ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientRelationXrefID"></param>
        /// <returns></returns>
        [HttpGet("Client/{clientID}/Relationship/{clientRelationXrefID?}")]
        public async Task<ActionResult<MessageHistory>> GetClientMessageHistoryByIDAsync(Guid clientID, Guid? clientRelationXrefID)
        {
            try
            {
                MessageHistory listLogicMessages = await repository.GetChatHistoryByClientIDAndOptionalRelationXrefIDAsync(clientID, clientRelationXrefID);
                return Ok(listLogicMessages);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
        // GET: api/History/Client/5
        /// <summary>
        /// Gets a list of message histories by a client's ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpGet("Client/{clientID}")]
        public async Task<ActionResult<List<Logic.Objects.MessageHistory>>> GetAllClientChatHistoriesAsync(Guid clientID)
        {
            try
            {
                List<MessageHistory> listLogicMessageHistory = await repository.GetAllChatHistoriesByClientIDAsync(clientID);
                return Ok(listLogicMessageHistory);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/History/Event/5
        /// <summary>
        /// Gets a list of message histories by a event's ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpGet("Event/{eventID}")]
        public async Task<ActionResult<List<Logic.Objects.MessageHistory>>> GetAllChatHistoriesByEventIDAsync(Guid eventID)
        {
            try
            {
                List<MessageHistory> listLogicMessageHistory = await repository.GetAllChatHistoriesByEventIDAsync(eventID);
                return Ok(listLogicMessageHistory);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/History/Unread
        /// <summary>
        /// Gets a list of message histories by a event's ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpGet("Event/Unread")]
        public async Task<ActionResult<List<Logic.Objects.MessageHistory>>> GetAllChatHistoriesWithUnreadMessagesAsync(Guid eventID)
        {
            try
            {
                List<MessageHistory> listLogicMessageHistory = await repository.GetAllChatHistoriesWithUnreadMessagesAsync();
                return Ok(listLogicMessageHistory);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}