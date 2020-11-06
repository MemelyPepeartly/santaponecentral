using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
        /// Gets all histories. Admin only
        /// </summary>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "read:histories")]
        public async Task<ActionResult<List<MessageHistory>>> GetAllHistoriesAsync([Required]Guid subjectID)
        {
            Client subjectClient = await repository.GetClientByIDAsync(subjectID);
            List<MessageHistory> listLogicHistory = await repository.GetAllChatHistories(subjectClient);
            return Ok(listLogicHistory);
        }

        // GET: api/History/Relationship/5
        /// <summary>
        /// Gets a specific history by XrefID with a subject to define who is the viewer
        /// </summary>
        /// <param name="subjectID"></param>
        /// <param name="clientRelationXrefID"></param>
        /// <returns></returns>
        [HttpGet("Relationship/{clientRelationXrefID}")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<MessageHistory>> GetClientMessageHistoryByXrefIDAndSubjectIDAsync(Guid clientRelationXrefID, [Required]Guid subjectID)
        {
            if(IsAuthorized(await repository.GetClientByIDAsync(subjectID)))
            {
                Client subjectClient = await repository.GetClientByIDAsync(subjectID);
                MessageHistory logicHistory = await repository.GetChatHistoryByXrefIDAndSubjectIDAsync(clientRelationXrefID, subjectClient);
                return Ok(logicHistory);
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
        }

        // GET: api/History/Client/5/General
        /// <summary>
        /// Gets a client's general history with a clientID to define as the viewer. Used for profiles and updating specific messages on the front end for soft refreshing
        /// ClientID in this endpoint is assumed to also be the subject of the conversation for determinging the viewer
        /// A given history
        /// </summary>
        /// <param name="conversationClientID"></param>
        /// <returns></returns>
        [HttpGet("Client/{conversationClientID}/General")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<MessageHistory>> GetClientGeneralMessageHistoryByClientIDAsync(Guid conversationClientID, Guid subjectID)
        {
            if (IsAuthorized(await repository.GetClientByIDAsync(subjectID)))
            {
                Client subjectClient = await repository.GetClientByIDAsync(subjectID);
                Client conversationClient = await repository.GetClientByIDAsync(conversationClientID);
                MessageHistory listLogicMessages = await repository.GetGeneralChatHistoryBySubjectIDAsync(conversationClient, subjectClient);
                return Ok(listLogicMessages);
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
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
            if (IsAuthorized(await repository.GetClientByIDAsync(subjectID)))
            {
                Client subjectClient = await repository.GetClientByIDAsync(subjectID);
                List<MessageHistory> listLogicMessageHistory = await repository.GetAllChatHistoriesBySubjectIDAsync(subjectClient);
                return Ok(listLogicMessageHistory);
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

        }
        private bool IsAuthorized(Client logicClient)
        {
            // Gets the claims from the token
            string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            return claimEmail == logicClient.email;
        }
    }
}