using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// <param name="subjectID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "read:histories")]
        public async Task<ActionResult<List<MessageHistory>>> GetAllHistoriesAsync([Required]Guid subjectID)
        {
            try
            {
                List<MessageHistory> listLogicHistory = await repository.GetAllChatHistories(subjectID);
                return Ok(listLogicHistory);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        // GET: api/History/Relationship/5
        /// <summary>
        /// Gets a specific history by xrefID with a subject to define who is the viewer
        /// </summary>
        /// <param name="subjectID"></param>
        /// <param name="clientRelationXrefID"></param>
        /// <returns></returns>
        [HttpGet("Relationship/{clientRelationXrefID}")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<MessageHistory>> GetClientMessageHistoryByXrefIDAndSubjectIDAsync(Guid clientRelationXrefID, Guid subjectID)
        {
            try
            {
#warning This controller can be used by clients. Need a check on the token claims that the requested clientID is the client selected, likely with a seperate task to get their profile and compare their email with the email on their token

                MessageHistory logicHistory = await repository.GetChatHistoryByXrefIDAndSubjectIDAsync(clientRelationXrefID, subjectID);
                return Ok(logicHistory);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/History/Client/5/Relationship/5
        /// <summary>
        /// Gets a client's general history with a clientID to define as the viewer. Used for profiles and updating specific messages on the front end for soft refreshing
        /// ClientID in this endpoint is assumed to also be the subject of the conversation for determinging the viewer
        /// A given history
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpGet("Client/{subjectID}/General")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<MessageHistory>> GetClientGeneralMessageHistoryByClientIDAsync(Guid clientID)
        {
            try
            {
#warning This controller can also be used by a client. Use the claims and the email in it to confirm that the requested clientID is from the client requesting
                MessageHistory listLogicMessages = await repository.GetGeneralChatHistoryBySubjectIDAsync(clientID);
                return Ok(listLogicMessages);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/History/Client/5/Relationship
        /// <summary>
        /// Gets a list of message histories by a subject's ID as the viewer for a profile. Gets all the chats of their assignments
        /// </summary>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        [HttpGet("Client/{subjectID}/Relationship")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<List<Logic.Objects.MessageHistory>>> GetAllClientChatHistoriesAsync(Guid subjectID)
        {
            try
            {
#warning Clients and admins can both use this, and is used for profile generation. Ensure the requesting client is only getting chats from their own profile.
                List<MessageHistory> listLogicMessageHistory = await repository.GetAllChatHistoriesBySubjectIDAsync(subjectID);
                return Ok(listLogicMessageHistory);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}