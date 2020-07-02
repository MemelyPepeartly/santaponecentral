using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HistoryController : ControllerBase
    {
        private readonly IRepository repository;
        public HistoryController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));

        }

        // GET: api/History
        /// <summary>
        /// Gets all histories
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientRelationXrefID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "read:histories")]
        public async Task<ActionResult<List<MessageHistory>>> GetAllHistoriesAsync()
        {
            try
            {
                List<MessageHistory> listLogicHistory = await repository.GetAllChatHistories();
                return Ok(listLogicHistory);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        // GET: api/History/Client/5/Relationship/5
        /// <summary>
        /// Gets a specific history by clientID and an optional relationship ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientRelationXrefID"></param>
        /// <returns></returns>
        [HttpGet("Client/{clientID}/Relationship/{clientRelationXrefID}")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<MessageHistory>> GetClientMessageHistoryByXrefIDAndClientIDAsync(Guid clientID, Guid clientRelationXrefID)
        {
            try
            {
#warning This controller can be used by clients. Need a check on the token claims that the requested clientID is the client selected, likely with a seperate task to get their profile and compare their email with the email on their token

                MessageHistory listLogicMessages = await repository.GetChatHistoryByClientIDAndRelationXrefIDAsync(clientID, clientRelationXrefID);
                return Ok(listLogicMessages);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/History/Client/5/Relationship/5
        /// <summary>
        /// Gets a specific history by clientID and an optional relationship ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientRelationXrefID"></param>
        /// <returns></returns>
        [HttpGet("Client/{clientID}/General")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<MessageHistory>> GetClientGeneralMessageHistoryByClientIDAsync(Guid clientID)
        {
            try
            {
#warning This controller can also be used by a client. Use the claims and the email in it to confirm that the requested clientID is from the client requesting
                MessageHistory listLogicMessages = await repository.GetGeneralChatHistoryByClientIDAsync(clientID);
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
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<List<Logic.Objects.MessageHistory>>> GetAllClientChatHistoriesAsync(Guid clientID)
        {
            try
            {
#warning Clients and admins can both use this, and is used for profile generation. Ensure the requesting client is only getting chats from their own profile.
                List<MessageHistory> listLogicMessageHistory = await repository.GetAllChatHistoriesByClientIDAsync(clientID);
                return Ok(listLogicMessageHistory);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/History/Event
        /// <summary>
        /// Gets a list of all event histories
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpGet("Event")]
        [Authorize(Policy = "read:histories")]
        public async Task<ActionResult<List<Logic.Objects.MessageHistory>>> GetAllEventChatHistoriesAsync()
        {
            try
            {
                List<Event> logicEvents = await repository.GetAllEvents();

                List<MessageHistory> listLogicMessageHistory = new List<MessageHistory>();
                foreach (Event logicEvent in logicEvents)
                {
                    listLogicMessageHistory.AddRange(await repository.GetAllChatHistoriesByEventIDAsync(logicEvent.eventTypeID));
                }

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
        [Authorize(Policy = "read:histories")]
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
    }
}