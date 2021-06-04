﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profile.Api.Models.Profile_Models;
using Profile.Logic.Constants;
using Profile.Logic.Interfaces;
using Profile.Logic.Models.Common_Models;
using Profile.Logic.Objects;
using Profile.Logic.Objects.Information_Objects;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Profile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProfileController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly ISharkTank sharkTank;
        public ProfileController(IRepository _repository, ISharkTank _sharktank)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            sharkTank = _sharktank ?? throw new ArgumentNullException(nameof(_sharktank));
        }

        // GET: api/Profile/email@domain.com/GetID
        /// <summary>
        /// Gets a client's profile by their email
        /// 
        /// Conditions: Have an Auth0 account, implying you have been approved
        /// </summary>
        /// <param email="email"></param>
        /// <returns></returns>
        [HttpGet("{email}/GetID")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<Logic.Objects.Profile>> GetClientIDForProfile(string email)
        {
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            BaseClient requestedClientInformation = await repository.GetBasicClientInformationByEmail(email);

            SharkTankValidationResponseModel sharkTankValidationModel = await sharkTank.CheckIfValidRequest(await makeSharkTankValidationModel(Method.GET, SharkTankConstants.GET_PROFILE_CATEGORY, requestedClientInformation.clientID));


            //if (logicBaseClient.clientID == checkerClient.clientID)
            if(sharkTankValidationModel.isValid)
            {
                return Ok(requestedClientInformation.clientID);
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
        }

        // GET: api/Profile/Email/email@domain.com
        /// <summary>
        /// Gets a client's profile by their email
        /// 
        /// Conditions: Have an Auth0 account, implying you have been approved
        /// </summary>
        /// <param email="email"></param>
        /// <returns></returns>
        [HttpGet("Email/{email}")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<Logic.Objects.Profile>> GetProfileByEmailAsync(string email)
        {
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            BaseClient requestedClientInformation = await repository.GetBasicClientInformationByEmail(email);

            SharkTankValidationResponseModel sharkTankValidationModel = await sharkTank.CheckIfValidRequest(await makeSharkTankValidationModel(Method.GET, SharkTankConstants.GET_PROFILE_CATEGORY, requestedClientInformation.clientID));
            Logic.Objects.Profile logicProfile = new Logic.Objects.Profile();
            logicProfile.sharkTankValidationResponse = sharkTankValidationModel;
            try
            {
                if(sharkTankValidationModel.isValid)
                {
                    logicProfile = await repository.GetProfileByEmailAsync(email);
                    
                    return Ok(logicProfile);
                }
                else
                {
                    if (!sharkTankValidationModel.isValid)
                    {
                        return Unauthorized(logicProfile);
                    }
                    else if (!sharkTankValidationModel.isRequestSuccess)
                    {
                        return UnprocessableEntity(logicProfile);
                    }
                }
            }
            // If it fails, log the error instead and stop the transaction
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest(logicProfile);
        }

        // GET: api/Profile/5
        /// <summary>
        /// Gets a client's profile by their ID
        /// </summary>
        /// <param clientID="clientID"></param>
        /// <returns></returns>
        [HttpGet("{clientID}")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<Logic.Objects.Profile>> GetProfileByIDAsync(Guid clientID)
        {
            /*
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            BaseClient checkerClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            Profile logicProfile = await repository.GetProfileByIDAsync(clientID);

            // Log the profile request
            try
            {
                await yuleLogger.logGetProfile(checkerClient, logicProfile);
            }
            // If it fails, log the error instead and stop the transaction
            catch (Exception)
            {
                await yuleLogger.logError(checkerClient, LoggingConstants.GET_PROFILE_CATEGORY);
                return StatusCode(StatusCodes.Status424FailedDependency);
            }

            if (logicProfile.clientID == checkerClient.clientID)
            {
                return Ok(logicProfile);
            }
            else
            {
                await yuleLogger.logError(checkerClient, LoggingConstants.GET_PROFILE_CATEGORY);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            */
            return StatusCode(StatusCodes.Status501NotImplemented);

        }

        // GET: api/Profile/5/Assignments
        /// <summary>
        /// Gets a client's assignments
        /// </summary>
        /// <param email="email"></param>
        /// <returns></returns>
        [HttpGet("{clientID}/Assignments")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<Logic.Objects.Profile>> GetAssignments(Guid clientID)
        {
            /*
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            BaseClient checkerClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            BaseClient logicStaticClient = await repository.GetBasicClientInformationByID(clientID);

            if (logicStaticClient.clientID == checkerClient.clientID)
            {
                return Ok((await repository.GetProfileAssignments(clientID)).OrderBy(pr => pr.recipientClient.clientNickname));
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            */
            return StatusCode(StatusCodes.Status501NotImplemented);

        }

        // GET: api/Profile/5/UnloadedHistories
        /// <summary>
        /// Gets a client's assignments
        /// </summary>
        /// <param email="email"></param>
        /// <returns></returns>
        [HttpGet("{clientID}/UnloadedHistories")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<List<MessageHistory>>> GetUnloadedHistories(Guid clientID)
        {
            /*
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            BaseClient checkerClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            BaseClient logicStaticClient = await repository.GetBasicClientInformationByID(clientID);

            if (logicStaticClient.clientID == checkerClient.clientID)
            {
                return Ok((await repository.GetUnloadedProfileChatHistoriesAsync(checkerClient.clientID)).OrderByDescending(h => h.eventType.eventDescription).ThenBy(h => h.assignmentRecieverClient.clientNickname));
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            */
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // PUT: api/Profile/5/Address
        /// <summary>
        /// Allows a client who owns the email and profile to update their address
        /// 
        /// Conditions: Have an Auth0 account, implying you have been approved
        /// </summary>
        /// <param clientID="clientID"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Address")]
        [Authorize(Policy = "update:profile")]
        public async Task<ActionResult<Logic.Objects.Profile>> UpdateProfileAddressAsync(Guid clientID, [FromBody] EditClientAddressModel newAddress)
        {
            /*
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            BaseClient checkerClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            Profile logicProfile = await repository.GetProfileByIDAsync(clientID);

            // Log the profile request
            try
            {
                await yuleLogger.logModifiedProfile(checkerClient, logicProfile);
            }
            // If it fails, log the error instead and stop the transaction
            catch (Exception)
            {
                await yuleLogger.logError(checkerClient, LoggingConstants.MODIFIED_PROFILE_CATEGORY);
                return StatusCode(StatusCodes.Status424FailedDependency);
            }

            // Checks to make sure the token's client is only getting the info for its own profile
            if (logicProfile.clientID == checkerClient.clientID)
            {
                Client logicClient = await repository.GetClientByIDAsync(clientID);
                logicClient.address = new Address()
                {
                    addressLineOne = newAddress.clientAddressLine1,
                    addressLineTwo = newAddress.clientAddressLine2,
                    city = newAddress.clientCity,
                    state = newAddress.clientState,
                    country = newAddress.clientCountry,
                    postalCode = newAddress.clientPostalCode
                };

                try
                {
                    await repository.UpdateClientByIDAsync(logicClient);
                    await repository.SaveAsync();
                    return Ok(await repository.GetProfileByIDAsync(checkerClient.clientID));
                }
                catch (Exception)
                {
                    await yuleLogger.logError(checkerClient, LoggingConstants.MODIFIED_PROFILE_CATEGORY);
                    return StatusCode(StatusCodes.Status424FailedDependency);
                }
            }
            else
            {
                await yuleLogger.logError(checkerClient, LoggingConstants.MODIFIED_PROFILE_CATEGORY);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            */
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // PUT: api/Profile/5/Assignment/5/AssignmentStatus
        /// <summary>
        /// Updates an assignment status on a profile
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="assignmentXrefID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Assignment/{assignmentXrefID}/AssignmentStatus")]
        [Authorize(Policy = "update:profile")]
        public async Task<ActionResult<AssignmentStatus>> UpdateProfileAssignmentStatus(Guid clientID, Guid assignmentXrefID, [FromBody] EditProfileAssignmentStatusModel model)
        {
            /*
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            Profile logicProfile = await repository.GetProfileByIDAsync(clientID);
            BaseClient baseCheckerClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            List<Guid> assignmentInfo = await repository.getClientAssignmentXrefIDsByIDAsync(baseCheckerClient.clientID);

            // If the checked client is allowed based on token claims, and the checker client has an assignment ID that match the one in the URI (To stop modifying of anyone elses assignments)
            if (logicProfile.clientID == baseCheckerClient.clientID && assignmentInfo.Any(crxr => crxr == assignmentXrefID))
            {
                RelationshipMeta assignment = await repository.getAssignmentRelationshipMetaByIDAsync(assignmentXrefID);
                AssignmentStatus newStatus = await repository.GetAssignmentStatusByID(model.assignmentStatusID);

                try
                {
                    // Update profile and send back the updated recipient
                    await repository.UpdateAssignmentProgressStatusByID(assignmentXrefID, model.assignmentStatusID);
                    await yuleLogger.logModifiedAssignmentStatus(baseCheckerClient, assignment.relationshipClient.clientNickname, assignment.assignmentStatus, newStatus);
                    await repository.SaveAsync();

                    RelationshipMeta newAssignmentMeta = await repository.getAssignmentRelationshipMetaByIDAsync(assignmentXrefID);

                    return Ok(newAssignmentMeta.assignmentStatus);
                }
                catch (Exception)
                {
                    await yuleLogger.logError(baseCheckerClient, LoggingConstants.MODIFIED_ASSIGNMENT_STATUS_CATEGORY);
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

        #region UTILITY
        private async Task<SharkTankValidationModel> makeSharkTankValidationModel(Method httpMethod, string requestedObjectCategory, Guid? validationID)
        {
            SharkTankValidationModel model = new SharkTankValidationModel()
            {
                requestorClientID = (await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value)).clientID,
                requestorRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList(),
                requestedObjectCategory = requestedObjectCategory,
                validationID = validationID != null ? validationID.Value : null,
                httpMethod = httpMethod
            };
            return model;
        }
        #endregion
    }
}
