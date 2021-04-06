using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharkTank.Api.Models.Auth0_Response_Models;
using SharkTank.Logic.Interfaces;
using SharkTank.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SharkTank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IAuthHelper authHelper;
        //private readonly IMailbag mailbag;
        private readonly IYuleLog yuleLogger;

        public AuthController(IRepository _repository, IAuthHelper _authHelper,/* IMailbag _mailbag, */IYuleLog _yuleLogger)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            authHelper = _authHelper ?? throw new ArgumentNullException(nameof(_authHelper));
            //mailbag = _mailbag ?? throw new ArgumentNullException(nameof(_mailbag));
            yuleLogger = _yuleLogger ?? throw new ArgumentNullException(nameof(_yuleLogger));
        }

        // GET: api/<AuthController>/<clientID>/Info
        /// <summary>
        /// Gets the auth0 info of a client by clientID
        /// </summary>
        /// <returns></returns>
        [HttpGet("{clientEmail}/Info")]
        public async Task<ActionResult<Auth0UserInfoModel>> Get(string clientEmail)
        {
            // Gets the claims from the URI and check against the client gotten based on auth claims token
            BaseClient logicBaseClient = await repository.GetBasicClientInformationByEmail(clientEmail);
            //BaseClient checkerClient = await repository.GetBasicClientInformationByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            if (true)
            //if (logicBaseClient.clientID == checkerClient.clientID)
            {
                return Ok(await authHelper.getAuthClientByEmail(clientEmail));
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
        }

        // POST: api/<AuthController>/New
        /// <summary>
        /// Creates a new auth user
        /// </summary>
        /// <returns></returns>
        [HttpPost("New")]
        public IEnumerable<string> PostNewClient()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/<AuthController>/Password
        /// <summary>
        /// Processes a password reset request
        /// </summary>
        /// <returns></returns>
        [HttpPost("Password")]
        public IEnumerable<string> PostPasswordReset()
        {
            return new string[] { "value1", "value2" };
        }

        // PUT: api/<AuthController>/<clientID>/Email
        /// <summary>
        /// Edits a client's email with auth
        /// </summary>
        /// <returns></returns>
        [HttpPut("{clientID}/Email")]
        public IEnumerable<string> PutEmail()
        {
            return new string[] { "value1", "value2" };
        }

        // PUT: api/<AuthController>/<clientID>/Name
        /// <summary>
        /// Edits a client's name with auth
        /// </summary>
        /// <returns></returns>
        [HttpPut("{clientID}/Name")]
        public IEnumerable<string> PutName()
        {
            return new string[] { "value1", "value2" };
        }

        // DELETE: api/<AuthController>/<clientID>/Auth
        /// <summary>
        /// Deletes a user's auth0 account
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{clientID}/Auth")]
        public IEnumerable<string> DeleteAuthClient()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
