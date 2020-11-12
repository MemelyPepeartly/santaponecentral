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
using Santa.Api.Services.YuleLog;
using Santa.Logic.Constants;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using Santa.Logic.Objects.Information_Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HistoryController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IYuleLog yuleLogger;
        public HistoryController(IRepository _repository, IYuleLog _yuleLogger)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            yuleLogger = _yuleLogger ?? throw new ArgumentNullException(nameof(_yuleLogger));
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
            BaseClient requestingClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            Client subjectClient = await repository.GetClientByIDAsync(subjectID);
            if(requestingClient.clientID == subjectID)
            {
                try
                {
                    await yuleLogger.logGetAllHistories(requestingClient);
                    List<MessageHistory> listLogicHistory = await repository.GetAllChatHistories(subjectClient);
                    return Ok(listLogicHistory);
                }
                catch(Exception)
                {
                    await yuleLogger.logError(requestingClient, LoggingConstants.GET_ALL_HISTORY_CATEGORY);
                    return StatusCode(StatusCodes.Status424FailedDependency);
                }
            }
            else
            {
                await yuleLogger.logError(requestingClient, LoggingConstants.GET_ALL_HISTORY_CATEGORY);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

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
            BaseClient requestingClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            Client subjectClient = await repository.GetClientByIDAsync(subjectID);
            
            if (requestingClient.email == subjectClient.email)
            {
                try
                {
                    MessageHistory logicHistory = await repository.GetChatHistoryByXrefIDAndSubjectIDAsync(clientRelationXrefID, subjectClient);
                    await yuleLogger.logGetSpecificHistory(requestingClient, logicHistory);
                    return Ok(logicHistory);
                }
                catch(Exception)
                {
                    await yuleLogger.logError(requestingClient, LoggingConstants.GET_ALL_HISTORY_CATEGORY);
                    return StatusCode(StatusCodes.Status424FailedDependency);
                }
                
            }
            else
            {
                await yuleLogger.logError(requestingClient, LoggingConstants.GET_SPECIFIC_HISTORY_CATEGORY);
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
            //Subject viewer client
            Client subjectClient = await repository.GetClientByIDAsync(subjectID);
            //Conversation the chat is with (If on profiles, will be the same as subject)
            Client conversationClient = await repository.GetClientByIDAsync(conversationClientID);
            //Client object based on token claim
            BaseClient checkerClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            // If the client isn't authorized, they get a 401, and a prompt spritz from the security spray bottle 
            if (checkerClient.email != subjectClient.email)
            {
                await yuleLogger.logError(checkerClient, LoggingConstants.GET_SPECIFIC_HISTORY_CATEGORY);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            // If the client is an admin, or the conversationClientID and subjectID are equal (Implying that they are just looking at their own general chat on profile)
            if(checkerClient.isAdmin || (conversationClientID == subjectID && checkerClient.clientID == subjectID))
            {
                try
                {
                    MessageHistory logicHistory = await repository.GetGeneralChatHistoryBySubjectIDAsync(conversationClient, subjectClient);
                    await yuleLogger.logGetSpecificHistory(checkerClient, logicHistory);
                    return Ok(logicHistory);
                }
                catch(Exception)
                {
                    await yuleLogger.logError(checkerClient, LoggingConstants.GET_ALL_HISTORY_CATEGORY);
                    return StatusCode(StatusCodes.Status424FailedDependency);
                }

            }
            await yuleLogger.logError(checkerClient, LoggingConstants.GET_SPECIFIC_HISTORY_CATEGORY);
            return StatusCode(StatusCodes.Status401Unauthorized);
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
            Client subjectClient = await repository.GetClientByIDAsync(subjectID);
            BaseClient requestingClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            if (requestingClient.email == subjectClient.email)
            {
                List<MessageHistory> listLogicMessageHistory = await repository.GetAllChatHistoriesBySubjectIDAsync(subjectClient);
                return Ok(listLogicMessageHistory);
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
        }
    }
}