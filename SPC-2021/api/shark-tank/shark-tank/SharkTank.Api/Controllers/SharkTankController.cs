using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharkTank.Api.Filters;
using SharkTank.Logic.Constants;
using SharkTank.Logic.Interfaces;
using SharkTank.Logic.Models.Auth0_Response_Models;
using SharkTank.Logic.Models.Common_Models;
using SharkTank.Logic.Objects;
using SharkTank.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SharkTank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharkTankController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IAuthHelper authHelper;
        //private readonly IMailbag mailbag;
        private readonly IYuleLog yuleLogger;

        public SharkTankController(IRepository _repository, IAuthHelper _authHelper,/* IMailbag _mailbag, */IYuleLog _yuleLogger)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            authHelper = _authHelper ?? throw new ArgumentNullException(nameof(_authHelper));
            //mailbag = _mailbag ?? throw new ArgumentNullException(nameof(_mailbag));
            yuleLogger = _yuleLogger ?? throw new ArgumentNullException(nameof(_yuleLogger));
        }

        // POST: api/<SharkTankController>/AuthInfo
        /// <summary>
        /// Endpoint gets the auth0UserInfoModel of all the requested Auth0 information
        /// </summary>
        /// <param name="requestInfo"></param>
        /// <returns></returns>
        [HttpPost("AuthInfo")]
        public async Task<ActionResult<Auth0UserInfoModel>> AuthInfo([FromBody] Auth0InfoRequestModel requestInfo)
        {
            List<Auth0RoleModel> requestingClientRoles = new List<Auth0RoleModel>();
            // If all the requesting client information or all the requested client information is null
            if((requestInfo.requestingClientID == null && String.IsNullOrEmpty(requestInfo.requestingClientEmail)) || (requestInfo.requestedClientID == null && String.IsNullOrEmpty(requestInfo.requestedClientEmail)))
            {
                return BadRequest("More information needed from either requesting client or requested client");
            }
            // If the requesting client has an ID
            if(requestInfo.requestingClientID != null)
            {
                Auth0UserInfoModel requestingClient = await authHelper.getAuthClientByEmail((await repository.GetBasicClientInformationByID((Guid)requestInfo.requestingClientID)).email);
                requestingClientRoles = await authHelper.getAllClientRolesByID(requestingClient.user_id);

                // If the client requested has an ID
                if (requestInfo.requestedClientID != null)
                {
                    // If the requesting client is an admin
                    if (requestingClientRoles.Any(r => r.description == Auth0Constants.EVENT_ADMIN || r.description == Auth0Constants.SANTADEV))
                    {
                        return Ok(await authHelper.getAuthClientByEmail((await repository.GetBasicClientInformationByID((Guid)requestInfo.requestedClientID)).email));
                    }
                    // Else if the requesting client is a standard user or helper
                    else if (requestingClientRoles.Any(r => r.description == Auth0Constants.PARTICIPANT || r.description == Auth0Constants.HELPER))
                    {
                        BaseClient wantedClient = await repository.GetBasicClientInformationByID((Guid)requestInfo.requestedClientID);
                        // If the wanted client's email is the requesting client's email, return the information
                        if (wantedClient.email == requestingClient.email)
                        {
                            return Ok(await authHelper.getAuthClientByEmail(wantedClient.email));
                        }
                        // Else, return unauthorized
                        else
                        {
                            return Unauthorized($"{requestingClient.nickname} cannot access information for {requestInfo.requestedClientID}");
                        }
                    }
                }
                // Else if the requested client has an email
                else if (!String.IsNullOrEmpty(requestInfo.requestedClientEmail))
                {
                    // If the requesting client is an admin
                    if (requestingClientRoles.Any(r => r.description == Auth0Constants.EVENT_ADMIN || r.description == Auth0Constants.SANTADEV))
                    {
                        return Ok(await authHelper.getAuthClientByEmail(requestInfo.requestedClientEmail));
                    }
                    // Else if the requesting client is a standard user or helper
                    else if (requestingClientRoles.Any(r => r.description == Auth0Constants.PARTICIPANT || r.description == Auth0Constants.HELPER))
                    {
                        // If the wanted client's email is the requesting client's email, return the information
                        if (requestInfo.requestedClientEmail == requestingClient.email)
                        {
                            return Ok(await authHelper.getAuthClientByEmail(requestInfo.requestedClientEmail));
                        }
                        // Else, return unauthorized
                        else
                        {
                            return Unauthorized($"{requestingClient.nickname} cannot access information for {requestInfo.requestedClientEmail}");
                        }
                    }
                }
            }
            // Else if the requesting client has an email
            else if (!String.IsNullOrEmpty(requestInfo.requestingClientEmail))
            {
                Auth0UserInfoModel requestingClient = await authHelper.getAuthClientByEmail(requestInfo.requestingClientEmail);
                requestingClientRoles = await authHelper.getAllClientRolesByID(requestingClient.user_id);
                
                // If the client requested has an ID
                if (requestInfo.requestedClientID != null)
                {
                    // If the requesting client is an admin
                    if (requestingClientRoles.Any(r => r.description == Auth0Constants.EVENT_ADMIN || r.description == Auth0Constants.SANTADEV))
                    {
                        return Ok(await authHelper.getAuthClientByEmail((await repository.GetBasicClientInformationByID((Guid)requestInfo.requestedClientID)).email));
                    }
                    // Else if the requesting client is a standard user or helper
                    else if(requestingClientRoles.Any(r => r.description == Auth0Constants.PARTICIPANT || r.description == Auth0Constants.HELPER))
                    {
                        BaseClient wantedClient = await repository.GetBasicClientInformationByID((Guid)requestInfo.requestedClientID);
                        // If the wanted client's email is the requesting client's email, return the information
                        if(wantedClient.email == requestingClient.email)
                        {
                            return Ok(await authHelper.getAuthClientByEmail(wantedClient.email));
                        }
                        // Else, return unauthorized
                        else
                        {
                            return Unauthorized($"{requestingClient.nickname} cannot access information for {requestInfo.requestedClientID}");
                        }
                    }
                }
                // Else if the requested client has an email
                else if (!String.IsNullOrEmpty(requestInfo.requestedClientEmail))
                {
                    // If the requesting client is an admin
                    if (requestingClientRoles.Any(r => r.description == Auth0Constants.EVENT_ADMIN || r.description == Auth0Constants.SANTADEV))
                    {
                        return Ok(await authHelper.getAuthClientByEmail(requestInfo.requestedClientEmail));
                    }
                    // Else if the requesting client is a standard user or helper
                    else if (requestingClientRoles.Any(r => r.description == Auth0Constants.PARTICIPANT || r.description == Auth0Constants.HELPER))
                    {
                        // If the wanted client's email is the requesting client's email, return the information
                        if (requestInfo.requestedClientEmail == requestingClient.email)
                        {
                            return Ok(await authHelper.getAuthClientByEmail(requestInfo.requestedClientEmail));
                        }
                        // Else, return unauthorized
                        else
                        {
                            return Unauthorized($"{requestingClient.nickname} cannot access information for {requestInfo.requestedClientEmail}");
                        }
                    }
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // POST api/<SharkTankController>
        /// <summary>
        /// Endpoint posts a new Auth0 user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Auth0UserInfoModel>> PostNewAuthUser([FromBody] Auth0CreateNewUserRequestModel model)
        {
            Auth0UserInfoModel newAuthClient =  await authHelper.createAuthClient(model.clientEmail, model.nickname);
            return Ok(newAuthClient);
        }

        // POST api/<SharkTankController>/Validate
        /// <summary>
        /// Endpoint validates if a request is allowed to be made by a user. Body includes the requesting client, and some identifying
        /// data they are wanting to request access to
        /// </summary>
        /// <param name="someObject"></param>
        /// <returns></returns>
        [HttpPost("Validate")]
        [SharkTankValidationFilter]
        public async Task<ActionResult<bool>> CheckIfValidRequest([FromBody] SharkTankValidationModel requestModel)
        {
            SharkTankValidationResponseModel response = new SharkTankValidationResponseModel()
            {
                isRequestSuccess = true
            };
            // If the requested object is empty
            if(String.IsNullOrEmpty(requestModel.requestedObjectCategory) && requestModel.requestedMessageHistoryContainer == null)
            {
                response.isRequestSuccess = false;
                response.isValid = false;
                response.errorMessages.Add(SharkTankErrorConstants.INVALID_REQUEST_ERROR);
                return Ok(response);
            }

            try
            {
                BaseClient requestingClient = await repository.GetBasicClientInformationByID(requestModel.requestorClientID);

                // If the request is for getting all clients
                if (requestModel.requestedObjectCategory == SharkTankConstants.GET_ALL_CLIENT_CATEGORY)
                {
                    // Log that a request is made to get all clients
                    await yuleLogger.logGetAllClients(requestingClient);

                    // If the requesting client is an admin or has the proper permissions
                    if (requestingClient.isAdmin || checkRoles(requestModel.requestorRoles, "read", "clients"))
                    {
                        // set to valid
                        response.isValid = true;
                    }
                    else
                    {
                        // log error (valid remains false)
                        await yuleLogger.logError(requestingClient, SharkTankConstants.GET_ALL_CLIENT_CATEGORY);
                    }
                }
                // Else if the request is for getting a profile
                else if(requestModel.requestedObjectCategory == SharkTankConstants.GET_PROFILE_CATEGORY)
                {
                    // Get the basic info the requested profile
                    BaseClient requestedClientInfo = await repository.GetBasicClientInformationByID(requestModel.requestedObjectID.Value);
                    // Log that a request is being made to get that profile
                    await yuleLogger.logGetProfile(requestingClient, requestedClientInfo);

                    // If the requesting client is accessing their own information, or the client is an admin
                    if((requestingClient.clientID == requestedClientInfo.clientID || requestingClient.isAdmin) && checkRoles(requestModel.requestorRoles, "read", "profile"))
                    {
                        // set to valid
                        response.isValid = true;
                    }
                    else
                    {
                        // log error (valid remains false)
                        await yuleLogger.logError(requestingClient, SharkTankConstants.GET_PROFILE_CATEGORY);
                    }
                }
                // Else if the request is for getting a message history
                else if (requestModel.requestedObjectCategory == SharkTankConstants.GET_SPECIFIC_HISTORY_CATEGORY)
                {
                    // Base client object for the subject client 
                    BaseClient subjectClient = await repository.GetBasicClientInformationByID(requestModel.requestedMessageHistoryContainer.subjectClientID);
                    // Get list of assignments the requestor has
                    List<RelationshipMeta> requestingClientAssignments = await repository.getClientAssignmentsInfoByIDAsync(requestingClient.clientID);
                    
                    // If the requesting client has the correct roles and the requested history is for an assignment
                    if(checkRoles(requestModel.requestorRoles, "read", "profile") && requestModel.requestedMessageHistoryContainer.assignmentXrefID != null)
                    {
                        // If the requesting client has any assignments that have the assignment xref ID requested for a history and the requestor has proper roles (Meaning they would be allowed to see that information)
                        if (requestingClientAssignments.Any<RelationshipMeta>(rm => rm.clientRelationXrefID == requestModel.requestedMessageHistoryContainer.assignmentXrefID))
                        {
                            // Log that a request for a specific history was made by requesting client, who was the subject, and who the assignment was about
                            await yuleLogger.logGetSpecificHistory(requestingClient, subjectClient, requestingClientAssignments.FirstOrDefault(am => am.clientRelationXrefID == requestModel.requestedMessageHistoryContainer.assignmentXrefID));
                            // set to valid
                            response.isValid = true;
                        }
                        else
                        {
                            // log error (valid remains false)
                            await yuleLogger.logError(requestingClient, SharkTankConstants.GET_SPECIFIC_HISTORY_CATEGORY);
                        }
                    }


                }
                // Else if the request is for posting a new message
                else if (requestModel.requestedObjectCategory == SharkTankConstants.CREATED_NEW_MESSAGE_CATEGORY)
                {
                    // Actions
                }
                return Ok(response);
            }
            catch(Exception e)
            {
                // Set request success to false and add errors
                response.isRequestSuccess = false;
                response.errorMessages.Add(SharkTankErrorConstants.FAILED_REQUEST_ERROR);
                response.errorMessages.Add(e.Message);

                return Ok(response);

            }

        }

        // PUT api/<SharkTankController>/5/Email
        /// <summary>
        /// Endpoint requests a change of a client's email for SPC and Auth0
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="emailChangeObject"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Email")]
        public async Task<ActionResult<Auth0UserInfoModel>> PutEmail(Guid clientID, [FromBody] object emailChangeObject)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // PUT api/<SharkTankController>/5/Name
        /// <summary>
        /// Endpoint requests a change of a client's holidayID for SPC and Auth0
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="emailChangeObject"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Name")]
        public async Task<ActionResult<Auth0UserInfoModel>> PutName(Guid clientID, [FromBody] object emailChangeObject)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // PUT api/<SharkTankController>/5/Password
        /// <summary>
        /// Endpoint requests a password reset ticket by a client's clientID
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Password")]
        public async Task<ActionResult<Auth0UserInfoModel>> PutTicketRequest(Guid clientID)
        {
            await authHelper.getPasswordChangeTicketByAuthClientEmail((await repository.GetBasicClientInformationByID((Guid)clientID)).email);
            return Ok(new Auth0UserInfoModel());
        }

        // DELETE api/<SharkTankController>/5
        /// <summary>
        /// Deletes a client's information on Auth0 by clientID. This will return true if sucessful
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpDelete("{clientID}")]
        public async Task<ActionResult<bool>> DeleteUser(Guid clientID)
        {
            Auth0UserInfoModel authUser = await authHelper.getAuthClientByEmail((await repository.GetBasicClientInformationByID(clientID)).email);
            await authHelper.deleteAuthClient(authUser.user_id);
            return Ok(true);
        }
        /// <summary>
        /// Checks the role the client has and compares them to an input access verb (such as "read") and the object type for role access (such as "clients"). 
        /// Will return true if the roles input has any roles that match the access verb and object type
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="requiredAccessVerb"></param>
        /// <param name="requiredObjectType"></param>
        /// <returns></returns>
        private bool checkRoles(List<Claim> roles, string requiredAccessVerb, string requiredObjectType)
        {
            bool isAllowed = false;

            if (roles.Any(r => r.Value == $"{requiredAccessVerb}:{requiredObjectType}"))
            {
                isAllowed = true;
            }
            return isAllowed;
        }
    }
}
