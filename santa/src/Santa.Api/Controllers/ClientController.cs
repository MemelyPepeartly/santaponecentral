﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Santa.Api.AuthHelper;
using Santa.Api.Models;
using Santa.Api.Models.Client_Models;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using Santa.Api.Constants;
using SendGrid;
using SendGrid.Helpers.Mail;
using Santa.Api.SendGrid;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientController : ControllerBase
    {

        private readonly IRepository repository;
        private readonly IAuthHelper authHelper;
        private readonly IMailbag mailbag;

        public ClientController(IRepository _repository, IAuthHelper _authHelper, IMailbag _mailbag)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            authHelper = _authHelper ?? throw new ArgumentNullException(nameof(_authHelper));
            mailbag = _mailbag ?? throw new ArgumentNullException(nameof(_mailbag));
        }
        // GET: api/Client
        /// <summary>
        /// Gets all clients
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "read:clients")]
        public async Task<ActionResult<List<Logic.Objects.Client>>> GetAllClients()
        {
            try
            {

                List <Logic.Objects.Client> clients = await repository.GetAllClients();
                if (clients == null)
                {
                    return NoContent();
                }
                return Ok(JsonConvert.SerializeObject(clients.OrderBy(c => c.nickname), Formatting.Indented));
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            
        }

        // GET: api/Client/5
        /// <summary>
        /// Gets a client by an ID
        /// 
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpGet("{clientID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "read:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> GetClientByIDAsync(Guid clientID)
        {
            try
            {
                return Ok(await repository.GetClientByIDAsync(clientID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            
        }


        // GET: api/Client/5/Response
        /// <summary>
        /// Gets a list of responses by a client's ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpGet("{clientID}/Response")]
        [Authorize(Policy = "read:clients")]
        public async Task<ActionResult<List<Logic.Objects.Client>>> GetClientResponsesByIDAsync(Guid clientID)
        {
            try
            {
                return Ok(await repository.GetAllSurveyResponsesByClientID(clientID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        // POST: api/Client
        /// <summary>
        /// Posts a new client. Binds the ApiClient model for input
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        //No authentication. New users with no account can post a client to the DB through the use of the sign up form
        public async Task<ActionResult<ApiClient>> PostAsync([FromBody, Bind("clientName, clientEmail, clientNickname, clientStatusID, clientAddressLine1, clientAddressLine2, clientCity, clientState, clientPostalCode, clientCountry")] ApiClient client)
        {
            try
            {
                Logic.Objects.Client newClient = new Logic.Objects.Client()
                {
                    clientID = Guid.NewGuid(),
                    clientName = client.clientName,
                    email = client.clientEmail,
                    nickname = client.clientNickname,
                    clientStatus = await repository.GetClientStatusByID(client.clientStatusID),
                    address = new Logic.Objects.Address()
                    {
                        addressLineOne = client.clientAddressLine1,
                        addressLineTwo = client.clientAddressLine2,
                        city = client.clientCity,
                        state = client.clientState,
                        postalCode = client.clientPostalCode,
                        country = client.clientCountry
                    },
                    recipients = new List<Recipient>(),
                    senders = new List<Sender>()
                };
                
                try
                {
                    await repository.CreateClient(newClient);
                    await repository.SaveAsync();
                    return Ok(await repository.GetClientByIDAsync(newClient.clientID));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }          
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        // POST: api/Client/5/Recipient
        [HttpPost("{clientID}/Recipient", Name = "PostRecipient")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PostRecipient(Guid clientID, [FromBody, Bind("clientNickname")] ApiClientRelationship relationship)
        {
            try
            {
                await repository.CreateClientRelationByID(clientID, relationship.recieverClientID, relationship.eventTypeID);
                await repository.SaveAsync();
                return Ok(await repository.GetClientByIDAsync(clientID));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        // POST: api/Client/5/Tag
        [HttpPost("{clientID}/Tag")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PostClientTagRelationship(Guid clientID, Guid tagID)
        {
            try
            {
                await repository.CreateClientTagRelationByID(clientID, tagID);
                await repository.SaveAsync();
                return Ok(await repository.GetClientByIDAsync(clientID));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // PUT: api/Client/5/Address
        [HttpPut("{clientID}/Address")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutAddress(Guid clientID, [FromBody, Bind("clientAddressLine1, clientAddressLine2, clientCity, clientState, clientPostalCode, clientCountry")] ApiClientAddress address)
        {
            try
            {
                try
                {
                    Logic.Objects.Client targetClient = await repository.GetClientByIDAsync(clientID);

                    targetClient.address.addressLineOne = address.clientAddressLine1;
                    targetClient.address.addressLineTwo = address.clientAddressLine2;
                    targetClient.address.city = address.clientCity;
                    targetClient.address.country = address.clientCountry;
                    targetClient.address.state = address.clientState;
                    targetClient.address.postalCode = address.clientPostalCode;

                    try
                    {
                        await repository.UpdateClientByIDAsync(targetClient);
                        await repository.SaveAsync();
                        Logic.Objects.Client updatedClient = await repository.GetClientByIDAsync(targetClient.clientID);
                        return Ok(updatedClient);
                    }
                    catch(Exception e)
                    {
                        throw e.InnerException;
                    }
                }
                catch(Exception e)
                {
                    throw e.InnerException;
                }
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        // PUT: api/Client/5/Email
        [HttpPut("{clientID}/Email", Name ="PutEmail")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutEmail(Guid clientID, [FromBody, Bind("clientEmail")] ApiClientEmail email)
        {
            try
            {
                Logic.Objects.Client targetClient = await repository.GetClientByIDAsync(clientID);
                Client oldClient = targetClient;
                targetClient.email = email.clientEmail;

                try
                {
                    await repository.UpdateClientByIDAsync(targetClient);
                    await repository.SaveAsync();
                    Logic.Objects.Client updatedClient = await repository.GetClientByIDAsync(targetClient.clientID);

                    try
                    {
                        // Gets the original client ID by the old email
                        Models.Auth0_Response_Models.Auth0UserInfoModel authClient = await authHelper.getAuthClientByEmail(oldClient.email);

                        // Updates a client's email and name in Auth0
                        await authHelper.updateAuthClientEmail(updatedClient.email, authClient.user_id);

                        // Sends the client a password change ticket
                        Models.Auth0_Response_Models.Auth0TicketResponse ticket = await authHelper.triggerPasswordChangeNotification(updatedClient.email);
                        await mailbag.sendPasswordResetEmail(oldClient, ticket, false);
                    }
                    catch(Exception e)
                    {
                        throw e.InnerException;
                    }

                    return Ok(updatedClient);
                }
                catch (Exception e)
                {
                    throw e.InnerException;
                }
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        // PUT: api/Client/5/Nickname
        [HttpPut("{clientID}/Nickname", Name = "PutNickname")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutNickname(Guid clientID, [FromBody, Bind("clientNickname")] ApiClientNickname nickname)
        {
            try
            {
                Logic.Objects.Client targetClient = await repository.GetClientByIDAsync(clientID);
                targetClient.nickname = nickname.clientNickname;

                try
                {
                    await repository.UpdateClientByIDAsync(targetClient);
                    await repository.SaveAsync();
                    Logic.Objects.Client updatedClient = await repository.GetClientByIDAsync(targetClient.clientID);
                    return Ok(updatedClient);
                }
                catch (Exception e)
                {
                    throw e.InnerException;
                }
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        // PUT: api/Client/5/Name
        [HttpPut("{clientID}/Name", Name = "PutName")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutName(Guid clientID, [FromBody, Bind("clientName")] ApiClientName name)
        {
            try
            {
                Logic.Objects.Client targetClient = await repository.GetClientByIDAsync(clientID);
                targetClient.clientName = name.clientName;
                try
                {
                    await repository.UpdateClientByIDAsync(targetClient);
                    await repository.SaveAsync();
                    Logic.Objects.Client updatedClient = await repository.GetClientByIDAsync(targetClient.clientID);
                    return Ok(updatedClient);
                }
                catch (Exception e)
                {
                    throw e.InnerException;
                }
                
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        // PUT: api/Client/5/Status
        [HttpPut("{clientID}/Status", Name = "PutStatus")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutStatus(Guid clientID, [FromBody, Bind("clientStatusID")] ApiClientStatus status)
        {
            try
            {
                // Grab original client
                Logic.Objects.Client targetClient = await repository.GetClientByIDAsync(clientID);
                Status originalStatus = targetClient.clientStatus;

                // Updates client status
                targetClient.clientStatus.statusID = status.clientStatusID;
                await repository.UpdateClientByIDAsync(targetClient);
                await repository.SaveAsync();

                // Get updated client
                Logic.Objects.Client updatedClient = await repository.GetClientByIDAsync(targetClient.clientID);
                try
                {
                    // Send approval steps for a client that was awaiting and approved for the event
                    if(updatedClient.clientStatus.statusDescription == Constants.Constants.APPROVED_STATUS && originalStatus.statusDescription == Constants.Constants.AWAITING_STATUS)
                    {
                        await ApprovalSteps(updatedClient);
                    }
                    // Send approval steps for client that was denied, and was accepted after appeal
                    else if(updatedClient.clientStatus.statusDescription == Constants.Constants.APPROVED_STATUS && originalStatus.statusDescription == Constants.Constants.DENIED_STATUS)
                    {
                        await mailbag.sendUndeniedEmail(updatedClient);
                        await ApprovalSteps(updatedClient);
                    }
                    // Send denied email to client that was awaiting and was denied
                    else if(updatedClient.clientStatus.statusDescription == Constants.Constants.DENIED_STATUS)
                    {
                        await mailbag.sendDeniedEmail(updatedClient);
                    }
                    return Ok(updatedClient);
                }
                catch (Exception e)
                {
                    throw e.InnerException;
                }

            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        // DELETE: api/Client/5
        [HttpDelete("{clientID}")]
        [Authorize(Policy = "delete:clients")]
        public async Task<ActionResult> Delete(Guid clientID)
        {
            try
            {
                await repository.DeleteClientByIDAsync(clientID);
                await repository.SaveAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        // DELETE: api/Client/5/Recipient
        [HttpDelete("{clientID}/Recipient")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> DeleteRecipientXref(Guid clientID, Guid recipientID, Guid eventID)
        {
            try
            {
                await repository.DeleteRecieverXref(clientID, recipientID, eventID);
                await repository.SaveAsync();
                return (await repository.GetClientByIDAsync(clientID));
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        // DELETE: api/Client/5/Tag
        [HttpDelete("{clientID}/Tag")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> DeleteClientTag(Guid clientID, Guid tagID)
        {
            try
            {
                await repository.DeleteClientTagRelationshipByID(clientID, tagID);
                await repository.SaveAsync();
                return (await repository.GetClientByIDAsync(clientID));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        private async Task ApprovalSteps(Client updatedClient)
        {
            // Creates auth client
            Models.Auth0_Response_Models.Auth0UserInfoModel authClient = await authHelper.createAuthClient(updatedClient.email);

            // Gets all the roles, and grabs the role for participants
            List<Models.Auth0_Response_Models.Auth0RoleModel> roles = await authHelper.getAllAuthRoles();
            Models.Auth0_Response_Models.Auth0RoleModel approvedRole = roles.First(r => r.name == Constants.Constants.PARTICIPANT);

            // Updates client with the participant role
            await authHelper.updateAuthClientRole(authClient.user_id, approvedRole.id);

            // Sends the client a password change ticket
            Models.Auth0_Response_Models.Auth0TicketResponse ticket = await authHelper.triggerPasswordChangeNotification(authClient.email);
            await mailbag.sendPasswordResetEmail(updatedClient, ticket, true);
        }
    }
}
