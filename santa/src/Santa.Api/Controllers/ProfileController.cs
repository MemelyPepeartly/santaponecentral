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
#warning Need protection here. Check request and make sure requesting email is only getting the profile for THEIR email. No fooling the DB here

                /*
                Microsoft.Extensions.Primitives.StringValues AuthHeaders = this.HttpContext.Request.Headers["Authorization"];
                string result = AuthHeaders[0].Substring(AuthHeaders[0].LastIndexOf(' ') + 1);
                JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = jwtHandler.ReadJwtToken(result);

                if (token.Audiences.Contains(email))
                {
                    //Logic here for checking the warning
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
                */

                Logic.Objects.Profile logicProfile = await repository.GetProfileByEmailAsync(email);
                List<Message> history = new List<Message>();

                // Finds all the messages that havn't been read yet and sets the number equal to how many messages have been unread for notification purposes
                foreach(ProfileRecipient recipient in logicProfile.recipients)
                {
                    history = await repository.GetChatHistory(logicProfile.clientID, recipient.relationXrefID);
                    foreach(Message message in history)
                    {
                        recipient.unreadCount += message.isMessageRead == false && message.recieverClient.clientId == logicProfile.clientID ? 1 : 0;
                    }
                }

                // Case is same as above, but rather for the one case where there is no relationship XrefID. Used for general chat
                history = await repository.GetChatHistory(logicProfile.clientID, null);
                foreach (Message message in history)
                {
                    logicProfile.generalChatUnreadCount += message.isMessageRead == false && message.recieverClient.clientId == logicProfile.clientID ? 1 : 0;
                }

                if (logicProfile == null)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(logicProfile);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
    }
}