using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Logic.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using Santa.Logic.Objects;
using System.Security.Claims;
using Santa.Api.Models;
using Santa.Api.Models.Profile_Models;
using Santa.Api.Services.YuleLog;
using Santa.Logic.Constants;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProfileController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IYuleLog yuleLogger;
        public ProfileController(IRepository _repository, IYuleLog _yuleLogger)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            yuleLogger = _yuleLogger ?? throw new ArgumentNullException(nameof(_yuleLogger));
        }

        // GET: api/Profile/email@domain.com
        /// <summary>
        /// Gets a client's profile by their email
        /// 
        /// Conditions: Have an Auth0 account, implying you have been approved
        /// </summary>
        /// <param email="email"></param>
        /// <returns></returns>
        [HttpGet("{email}")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<Logic.Objects.Profile>> GetProfileByEmailAsync(string email)
        {
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            Logic.Objects.Profile logicProfile = await repository.GetProfileByEmailAsync(email);
            Logic.Objects.Client checkerClient = await repository.GetClientByEmailAsync(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            
            // Log the profile request
            try
            {
                await yuleLogger.logGetProfile(checkerClient, logicProfile);
            }
            // If it fails, log the error instead and stop the transaction
            catch(Exception)
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
        }
        // GET: api/Profile/5/Address
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
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            Profile logicProfile = await repository.GetProfileByIDAsync(clientID);
            Logic.Objects.Client checkerClient = await repository.GetClientByEmailAsync(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

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
                catch(Exception)
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

        }
        [HttpPut("{clientID}/Assignment/{assignmentXrefID}/AssignmentStatus")]
        [Authorize(Policy = "update:profile")]
        public async Task<ActionResult<AssignmentStatus>> UpdateProfileAssignmentStatus(Guid clientID, Guid assignmentXrefID, [FromBody] EditProfileAssignmentStatusModel model)
        {
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            Profile logicProfile = await repository.GetProfileByIDAsync(clientID);
            Logic.Objects.Client checkerClient = await repository.GetClientByEmailAsync(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            // If the checked client is allowed based on token claims, and the checker client has an assignment ID that match the one in the URI (To stop modifying of anyone elses assignments)
            if (logicProfile.clientID == checkerClient.clientID && checkerClient.assignments.Any(a => a.clientRelationXrefID == assignmentXrefID))
            {
                RelationshipMeta assignment = checkerClient.assignments.First(a => a.clientRelationXrefID == assignmentXrefID);
                AssignmentStatus newStatus = await repository.GetAssignmentStatusByID(model.assignmentStatusID);

                try
                {
                    // Update profile and send back the updated recipient
                    await repository.UpdateAssignmentProgressStatusByID(assignmentXrefID, model.assignmentStatusID);
                    await yuleLogger.logModifiedAssignmentStatus(checkerClient, assignment.relationshipClient.clientNickname, assignment.assignmentStatus, newStatus);
                    await repository.SaveAsync();
                    
                    return Ok((await repository.GetProfileByIDAsync(checkerClient.clientID)).assignments.First(r => r.relationXrefID == assignmentXrefID).assignmentStatus);
                }
                catch(Exception)
                {
                    await yuleLogger.logError(checkerClient, LoggingConstants.MODIFIED_ASSIGNMENT_STATUS_CATEGORY);
                    return StatusCode(StatusCodes.Status424FailedDependency);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
        }
    }
}