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
            try
            {

                // Gets the claims from the token
                string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

                // Checks to make sure the token's email is only getting the email for its own profile. Takes one less call than using the IsAuthorized method here
                if (claimEmail == email)
                {
                    Logic.Objects.Profile logicProfile = await repository.GetProfileByEmailAsync(email);

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
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
        // GET: api/Profile/email@domain.com/Address
        /// <summary>
        /// Allows a client who owns the email and profile to update their address
        /// 
        /// Conditions: Have an Auth0 account, implying you have been approved
        /// </summary>
        /// <param clientID="clientID"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Address")]
        [Authorize(Policy = "update:profile")]
        public async Task<ActionResult<Logic.Objects.Profile>> UpdateProfileAddressByEmailAsync(Guid clientID, [FromBody] ApiClientAddressModel newAddress)
        {
            try
            {
                Client logicClient = await repository.GetClientByIDAsync(clientID);
                // Checks to make sure the token's email is only getting the email for its own profile
                if (IsAuthorized(User, logicClient))
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

                    Profile logicProfile = await repository.GetProfileByEmailAsync(logicClient.email);
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
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
        [HttpPut("{clientID}/Assignment/{assignmentXrefID}/AssignmentStatus")]
        [Authorize(Policy = "update:profile")]
        public async Task<ActionResult<AssignmentStatus>> UpdateProfileAssignmentStatus(Guid clientID, Guid assignmentXrefID, [FromBody] EditProfileAssignmentStatusModel model)
        {
            try
            {
                Client logicClient = await repository.GetClientByIDAsync(clientID);
                if (IsAuthorized(User, logicClient))
                {
                    // Logic needed here for updating assignment status
                    await repository.UpdateAssignmentProgressStatusByID(assignmentXrefID, model.assignmentStatusID);
                    await repository.SaveAsync();

                    // Update profile and send back the updated recipient
                    Profile logicProfile = await repository.GetProfileByEmailAsync(logicClient.email);
                    return Ok(logicProfile.recipients.First(r => r.relationXrefID == assignmentXrefID).assignmentStatus);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        private bool IsAuthorized(ClaimsPrincipal user, Client logicClient)
        {
            // Gets the claims from the token
            string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            return claimEmail == logicClient.email;
        }
    }
}