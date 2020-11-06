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

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProfileController : ControllerBase
    {
        private readonly IRepository repository;
        public ProfileController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));

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

            if (logicProfile.clientID == checkerClient.clientID)
            {
                if (logicProfile == null)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(logicProfile);
                }
            }
            else
            {
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
            Logic.Objects.Client logicClient = await repository.GetClientByIDAsync(clientID);
            Logic.Objects.Client checkerClient = await repository.GetClientByEmailAsync(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            // Checks to make sure the token's client is only getting the info for its own profile
            if (logicClient.clientID == checkerClient.clientID)
            {
                logicClient.address = new Address()
                {
                    addressLineOne = newAddress.clientAddressLine1,
                    addressLineTwo = newAddress.clientAddressLine2,
                    city = newAddress.clientCity,
                    state = newAddress.clientState,
                    country = newAddress.clientCountry,
                    postalCode = newAddress.clientPostalCode
                };
                await repository.UpdateClientByIDAsync(logicClient);
                await repository.SaveAsync();

                Profile logicProfile = await repository.GetProfileByIDAsync(checkerClient.clientID);
                if (logicProfile == null)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(logicProfile);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

        }
        [HttpPut("{clientID}/Assignment/{assignmentXrefID}/AssignmentStatus")]
        [Authorize(Policy = "update:profile")]
        public async Task<ActionResult<AssignmentStatus>> UpdateProfileAssignmentStatus(Guid clientID, Guid assignmentXrefID, [FromBody] EditProfileAssignmentStatusModel model)
        {
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            Logic.Objects.Client logicClient = await repository.GetClientByIDAsync(clientID);
            Logic.Objects.Client checkerClient = await repository.GetClientByEmailAsync(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            // If the checked client is allowed based on token claims, and the checker client has an assignment ID that match the one in the URI (To stop modifying of anyone elses assignments)
            if (logicClient.clientID == checkerClient.clientID && checkerClient.assignments.Any(a => a.clientRelationXrefID == assignmentXrefID))
            {
                // Logic needed here for updating assignment status
                await repository.UpdateAssignmentProgressStatusByID(assignmentXrefID, model.assignmentStatusID);
                await repository.SaveAsync();

                // Update profile and send back the updated recipient
                Profile logicProfile = await repository.GetProfileByEmailAsync(logicClient.email);
                return Ok(logicProfile.assignments.First(r => r.relationXrefID == assignmentXrefID).assignmentStatus);
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
        }
    }
}