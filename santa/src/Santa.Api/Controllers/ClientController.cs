using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Santa.Api.AuthHelper;
using Santa.Api.Models;
using Santa.Api.Models.Client_Models;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using Santa.Api.SendGrid;
using Santa.Logic.Constants;

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
        [Authorize(Policy = "create:clients")]
        public async Task<ActionResult<Client>> PostClientAsync([FromBody] ApiClient client)
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
        // POST: api/Client
        /// <summary>
        /// Posts a new client signup with their responses
        /// </summary>
        /// <param name="clientResponseModel"></param>
        /// <returns></returns>
        [HttpPost("Signup")]
        [AllowAnonymous]
        //No authentication. New users with no account can post a client to the DB through the use of the sign up form
        public async Task<ActionResult<Client>> PostSignupAsync([FromBody] ApiClientWithResponses clientResponseModel)
        {
            try
            {
                Logic.Objects.Client newClient = new Logic.Objects.Client()
                {
                    clientID = Guid.NewGuid(),
                    clientName = clientResponseModel.clientName,
                    email = clientResponseModel.clientEmail,
                    nickname = clientResponseModel.clientNickname,
                    clientStatus = await repository.GetClientStatusByID(clientResponseModel.clientStatusID),
                    address = new Logic.Objects.Address()
                    {
                        addressLineOne = clientResponseModel.clientAddressLine1,
                        addressLineTwo = clientResponseModel.clientAddressLine2,
                        city = clientResponseModel.clientCity,
                        state = clientResponseModel.clientState,
                        postalCode = clientResponseModel.clientPostalCode,
                        country = clientResponseModel.clientCountry
                    },
                    recipients = new List<Recipient>(),
                    senders = new List<Sender>()
                };

                try
                {
                    await repository.CreateClient(newClient);
                    foreach(Models.Survey_Response_Models.ApiSurveyResponse response in clientResponseModel.responses)
                    {
                        await repository.CreateSurveyResponseAsync(new Logic.Objects.Response()
                        {
                            surveyResponseID = Guid.NewGuid(),
                            surveyID = response.surveyID,
                            clientID = newClient.clientID,
                            surveyQuestion = new Question() { questionID = response.surveyQuestionID },
                            surveyOptionID = response.surveyOptionID,
                            responseText = response.responseText
                        });
                    }
                    await repository.SaveAsync();

                    return Ok(await repository.GetClientByIDAsync(newClient.clientID));
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        // POST: api/Client/5/Recipients
        /// <summary>
        /// Endpoint for posting more than one assignment at once
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="assignmentsModel"></param>
        /// <returns></returns>
        [HttpPost("{clientID}/Recipients", Name = "PostRecipients")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PostRecipient(Guid clientID, [FromBody] ApiClientRelationships assignmentsModel)
        {
            try
            {
                foreach(Guid assignmentID in assignmentsModel.assignments)
                {
                    await repository.CreateClientRelationByID(clientID, assignmentID, assignmentsModel.eventTypeID);
                }
                await repository.SaveAsync();

                // Get new client with recipients, and send the client a notification they have new assignments for an event
                Client updatedClient = await repository.GetClientByIDAsync(clientID);
                Event assigneeEvent = await repository.GetEventByIDAsync(assignmentsModel.eventTypeID);
                await mailbag.sendAssignedRecipientEmail(updatedClient, assigneeEvent);

                return Ok(updatedClient);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        // POST: api/Client/5/Tag
        /// <summary>
        /// Assigns a specific user a tag by ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="tagID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Updates a client's address
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Address")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutAddress(Guid clientID, [FromBody] ApiClientAddress address)
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
        /// <summary>
        /// Updates a client's email and Auth0 email, then sends them an option to reset their password
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Email", Name ="PutEmail")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutEmail(Guid clientID, [FromBody] ApiClientEmail email)
        {
            try
            {
                Logic.Objects.Client targetClient = await repository.GetClientByIDAsync(clientID);
                string oldEmail = targetClient.email;
                targetClient.email = email.clientEmail;

                try
                {
                    await repository.UpdateClientByIDAsync(targetClient);
                    await repository.SaveAsync();
                    Logic.Objects.Client updatedClient = await repository.GetClientByIDAsync(targetClient.clientID);

                    try
                    {
                        // Gets the original client ID by the old email
                        Models.Auth0_Response_Models.Auth0UserInfoModel authClient = await authHelper.getAuthClientByEmail(oldEmail);

                        // If the auth response is null, and the client was still awaiting, means that they didn't have an auth account yet. Return the update

                        if(string.IsNullOrEmpty(authClient.user_id) && updatedClient.clientStatus.statusDescription == Constants.AWAITING_STATUS)
                        {
                            return Ok(updatedClient);
                        }
                        // Else if the result is null but they weren't awaiting, something went wrong. Change the email back and send a bad request
                        else if(string.IsNullOrEmpty(authClient.user_id) && updatedClient.clientStatus.statusDescription != Constants.AWAITING_STATUS)
                        {
                            targetClient.email = oldEmail;
                            await repository.UpdateClientByIDAsync(targetClient);
                            await repository.SaveAsync();
                            return StatusCode(StatusCodes.Status400BadRequest, "Something went wrong. The user did not have an auth account to update");
                        }

                        // Updates a client's email and name in Auth0
                        await authHelper.updateAuthClientEmail(updatedClient.email, authClient.user_id);

                        // Sends the client a password change ticket
                        Models.Auth0_Response_Models.Auth0TicketResponse ticket = await authHelper.triggerPasswordChangeNotification(updatedClient.email);
                        await mailbag.sendPasswordResetEmail(oldEmail, updatedClient.nickname, ticket, false);

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
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        // PUT: api/Client/5/Nickname
        /// <summary>
        /// Update a client's nickname
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Nickname", Name = "PutNickname")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutNickname(Guid clientID, [FromBody] ApiClientNickname nickname)
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
        /// <summary>
        /// Updates a client's actual name
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Updates the status of a certain client by their ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Status", Name = "PutStatus")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutStatus(Guid clientID, [FromBody] ApiClientStatus status)
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
                    if(updatedClient.clientStatus.statusDescription == Constants.APPROVED_STATUS && originalStatus.statusDescription == Constants.AWAITING_STATUS)
                    {
                        await ApprovalSteps(updatedClient);
                    }
                    // Send approval steps for client that was denied, and was accepted after appeal
                    else if(updatedClient.clientStatus.statusDescription == Constants.APPROVED_STATUS && originalStatus.statusDescription == Constants.DENIED_STATUS)
                    {
                        await mailbag.sendUndeniedEmail(updatedClient);
                        await ApprovalSteps(updatedClient);
                    }
                    // Send denied email to client that was awaiting and was denied
                    else if(updatedClient.clientStatus.statusDescription == Constants.DENIED_STATUS)
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
        // PUT: api/Client/5/Recipient
        /// <summary>
        /// Updates the completion status of a relationship by sender, reciever, and event type ID's
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="recipientCompletionModel"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Recipient")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> UpdateRecipientXrefCompletionStatus(Guid clientID, [FromBody] ApiRecipientCompletionModel recipientCompletionModel)
        {
            try
            {
                if(recipientCompletionModel.recipientID.Equals(Guid.Empty) || recipientCompletionModel.eventTypeID.Equals(Guid.Empty))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "One or both of the required ID's for reciever or eventType were not present on the request");
                }
                else
                {
                    await repository.UpdateClientRelationCompletedStatusByID(clientID, recipientCompletionModel.recipientID, recipientCompletionModel.eventTypeID, recipientCompletionModel.completed);
                    await repository.SaveAsync();
                    return (await repository.GetClientByIDAsync(clientID));
                }
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        // DELETE: api/Client/5
        /// <summary>
        /// Deletes a client
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpDelete("{clientID}")]
        [Authorize(Policy = "delete:clients")]
        public async Task<ActionResult> Delete(Guid clientID)
        {
#warning Not Auth0 functioning yet
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
        /// <summary>
        /// Delets an assignment from a client
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="recipientID"></param>
        /// <param name="eventID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Deletes a tag from a client
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="tagID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method for approval steps
        /// </summary>
        /// <param name="logicClient"></param>
        /// <returns></returns>
        private async Task ApprovalSteps(Client logicClient)
        {
            // Creates auth client
            Models.Auth0_Response_Models.Auth0UserInfoModel authClient = await authHelper.createAuthClient(logicClient.email, logicClient.nickname);

            // Gets all the roles, and grabs the role for participants
            List<Models.Auth0_Response_Models.Auth0RoleModel> roles = await authHelper.getAllAuthRoles();
            Models.Auth0_Response_Models.Auth0RoleModel approvedRole = roles.First(r => r.name == Constants.PARTICIPANT);

            // Updates client with the participant role
            await authHelper.updateAuthClientRole(authClient.user_id, approvedRole.id);

            // Sends the client a password change ticket
            Models.Auth0_Response_Models.Auth0TicketResponse ticket = await authHelper.triggerPasswordChangeNotification(logicClient.email);
            await mailbag.sendPasswordResetEmail(logicClient.email, logicClient.nickname, ticket, true);
        }
    }
}
