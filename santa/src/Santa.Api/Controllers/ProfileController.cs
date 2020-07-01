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
        public async Task<ActionResult<Logic.Objects.Profile>> GetProfileByEmailAsync(string email)
        {
            try
            {

                // Gets the claims from the token
                string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

                // Checks to make sure the token's email is only getting the email for its own profile
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
    }
}