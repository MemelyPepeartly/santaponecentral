using Message.Logic.Constants;
using Message.Logic.Interfaces;
using Message.Logic.Models.Common_Models;
using Message.Logic.Objects;
using Message.Logic.Objects.Information_Objects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Message.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HistoryController : ControllerBase
    {
#warning All mailbag and YuleLog functionality will need to be updated from legacy to support microservice infrastructure

        private readonly IRepository repository;
        private readonly ISharkTank sharkTank;
        public HistoryController(IRepository _repository, ISharkTank _sharkTank)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            sharkTank = _sharkTank ?? throw new ArgumentNullException(nameof(_sharkTank));
        }

        // GET: api/History
        /// <summary>
        /// Gets all histories. Admin only
        /// </summary>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "read:histories")]
        public async Task<ActionResult<List<MessageHistory>>> GetAllHistoriesAsync([Required] Guid subjectID)
        {
            /*
            BaseClient requestingClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            BaseClient subjectClient = await repository.GetBasicClientInformationByID(subjectID);
            if (requestingClient.clientID == subjectID)
            {
                try
                {
                    await yuleLogger.logGetAllHistories(requestingClient);
                    List<MessageHistory> listLogicHistory = await repository.GetAllChatHistories(subjectClient);
                    return Ok(listLogicHistory);
                }
                catch (Exception)
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
            */
            return StatusCode(StatusCodes.Status501NotImplemented);

        }

        // GET: api/Client/5/EventHistory/5
        /// <summary>
        /// Gets a specific history for a client by event ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="eventTypeID"></param>
        /// <returns></returns>
        [HttpGet("Client/{clientID}/EventHistory/{eventTypeID}")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<MessageHistory>> GetMessageHistoryByRequestorClientIDAndEventTypeID(Guid clientID, Guid eventTypeID)
        {
            BaseClient URIClient = await repository.GetBasicClientInformationByID(clientID);

            try
            {
                MessageHistory logicHistory = new MessageHistory();
                SharkTankValidationResponseModel validationModel = await sharkTank.CheckIfValidRequest(await makeSharkTankValidationModel(Method.GET, SharkTankConstants.GET_SPECIFIC_HISTORY_CATEGORY, URIClient.clientID));

                if (validationModel.isValid)
                {
                    logicHistory = await repository.GetSpecificHistoryByClientIDAndEventID(URIClient.clientID, eventTypeID);
                    return Ok(logicHistory);
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status424FailedDependency);
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
            /*
            //Subject viewer client
            BaseClient subjectClient = await repository.GetBasicClientInformationByID(subjectID);
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
            if (checkerClient.isAdmin || (conversationClientID == subjectID && checkerClient.clientID == subjectID))
            {
                try
                {
                    MessageHistory logicHistory = await repository.GetGeneralChatHistoryBySubjectIDAsync(conversationClient, subjectClient);
                    await yuleLogger.logGetSpecificHistory(checkerClient, logicHistory);
                    return Ok(logicHistory);
                }
                catch (Exception)
                {
                    await yuleLogger.logError(checkerClient, LoggingConstants.GET_ALL_HISTORY_CATEGORY);
                    return StatusCode(StatusCodes.Status424FailedDependency);
                }

            }
            await yuleLogger.logError(checkerClient, LoggingConstants.GET_SPECIFIC_HISTORY_CATEGORY);
            return StatusCode(StatusCodes.Status401Unauthorized);
            */
            return StatusCode(StatusCodes.Status501NotImplemented);

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
            /*
            BaseClient subjectClient = await repository.GetBasicClientInformationByID(subjectID);
            BaseClient requestingClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            if (requestingClient.email == subjectClient.email)
            {
                List<MessageHistory> listLogicMessageHistory = await repository.GetAllAssignmentChatsByClientID(subjectClient);
                return Ok(listLogicMessageHistory);
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            */
            return StatusCode(StatusCodes.Status501NotImplemented);

        }

        #region UTILITY
        private async Task<SharkTankValidationModel> makeSharkTankValidationModel(Method httpMethod, string requestedObjectCategory, Guid? validationID)
        {
            SharkTankValidationModel model = new SharkTankValidationModel()
            {
                requestorClientID = (await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == "https://example.com/email").Value)).clientID,
                requestorRoles = User.Claims.Where(c => c.Type == "permissions").Select(claim => new PermissionClaim()
                {
                    Type = claim.Type,
                    Value = claim.Value
                }).ToList(),
                requestedObjectCategory = requestedObjectCategory,
                validationID = validationID != null ? validationID.Value : null,
                httpMethod = httpMethod
            };
            return model;
        }
        #endregion
    }
}
