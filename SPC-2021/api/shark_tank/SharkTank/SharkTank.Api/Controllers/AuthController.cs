using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharkTank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // GET: api/<AuthController>/<clientID>/Info
        /// <summary>
        /// Gets the auth0 info of a client by clientID
        /// </summary>
        /// <returns></returns>
        [HttpGet("{clientID}/Info")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
