using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharkTank.Logic.Models.Auth0_Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SharkTank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharkTankController : ControllerBase
    {
        // POST: api/<SharkTankController>
        /// <summary>
        /// Endpoint gets the auth0UserInfoModel of all their Auth0 information
        /// </summary>
        /// <param name="requestInfo"></param>
        /// <returns></returns>
        [HttpPost("AuthInfo")]
        public async Task<ActionResult<Auth0UserInfoModel>> AuthInfo([FromBody] object requestInfo)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // POST api/<SharkTankController>
        /// <summary>
        /// Endpoint posts a new Auth0 user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> PostNewAuthUser([FromBody] Auth0NewUserModel model)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // POST api/<SharkTankController>/Validate
        /// <summary>
        /// Endpoint validates if a request is allowed to be made by a user. Body includes the requesting client, and some identifying
        /// data they are wanting to request access to
        /// </summary>
        /// <param name="someObject"></param>
        /// <returns></returns>
        [HttpPost("Validate")]
        public async Task<ActionResult<bool>> CheckIfValidRequest([FromBody] object someObject)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
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
            return StatusCode(StatusCodes.Status500InternalServerError);
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
            return StatusCode(StatusCodes.Status500InternalServerError);
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
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // DELETE api/<SharkTankController>/5
        /// <summary>
        /// Deletes a client's information on SPC and Auth0 by clientID. This will return true if both actions were successful
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpDelete("{clientID}")]
        public async Task<ActionResult<bool>> DeleteUser(Guid clientID)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
