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
using Santa.Logic.Objects.Information_Objects;

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
                return Ok(clients.OrderBy(c => c.nickname));
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            
        }

        // GET: api/Client/5
        /// <summary>
        /// Gets a client by an ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpGet("{clientID}")]
        [Authorize(Policy = "read:clients")]
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

        // GET: api/Client/Email/email@domain.com
        /// <summary>
        /// Gets a client by an email
        /// </summary>
        /// <param name="clientEmail"></param>
        /// <returns></returns>
        [HttpGet("Email/{clientEmail}")]
        [Authorize(Policy = "read:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> GetClientByIDAsync(string clientEmail)
        {
            try
            {
                Client logicClient = await repository.GetClientByEmailAsync(clientEmail);
                return Ok(logicClient);
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

        // GET: api/Client/5/AllowedAssignment/5
        [HttpGet("{clientID}/AllowedAssignment/{eventTypeID}")]
        [Authorize(Policy = "read:clients")]
        public async Task<ActionResult<List<AllowedAssignmentMeta>>> GetAllAllowedAssignmentsForClientByEventID(Guid clientID, Guid eventTypeID)
        {
            try
            {
                return Ok(await repository.GetAllAllowedAssignmentsByID(clientID, eventTypeID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Client/AutoAssignmentPairs
        /// <summary>
        /// Endpoint for looking through who is a mass mailer (by tag), and returning a list of potential assignment pairings for mass mailers
        /// </summary>
        /// <returns></returns>
        [HttpGet("AutoAssignmentPairs")]
        [Authorize(Policy = "read:clients")]
        public async Task<ActionResult<List<PossiblePairing>>> GetAutoAssignmentsToMassMailerPairs()
        {
            try
            {
                List<Client> allClients = await repository.GetAllClients();
                List<Client> massMailers = allClients.Where(c => c.tags.Any(t => t.tagName == Constants.MASS_MAILER_TAG)).ToList();
                List<Client> clientsToBeAssignedToMassMailers = allClients.Where(c => c.tags.Any(t => t.tagName == Constants.MASS_MAIL_RECIPIENT_TAG)).ToList();
                AssignmentStatus defaultNewAssignmentStatus = (await repository.GetAllAssignmentStatuses()).First(stat => stat.assignmentStatusName == Constants.ASSIGNED_ASSIGNMENT_STATUS);

                List<PossiblePairing> possiblePairings = new List<PossiblePairing>();

                Event logicCardExchangeEvent = await repository.GetEventByNameAsync(Constants.CARD_EXCHANGE_EVENT);

                if (massMailers.Count > 0 && clientsToBeAssignedToMassMailers.Count > 0)
                {
                    // Foreach mailer
                    foreach (Client mailer in massMailers)
                    {
                        // Foreach clients to be assigned mass mail
                        foreach (Client potentialAssignment in clientsToBeAssignedToMassMailers)
                        {
                            // If the mass mailer doesnt already have the potential assignment in their assignments list, and they aren't themselves
                            if (!mailer.assignments.Any<RelationshipMeta>(c => c.relationshipClient.clientId == potentialAssignment.clientID) && mailer.clientID != potentialAssignment.clientID)
                            {
                                // Add the possible pairing to the list
                                possiblePairings.Add(new PossiblePairing()
                                {
                                    sendingAgent = mailer,
                                    possibleAssignment = potentialAssignment
                                });
                            }
                        }
                    }
                }

                return Ok(possiblePairings);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
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
        public async Task<ActionResult<Client>> PostClientAsync([FromBody] EditNewClientModel client)
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
                    assignments = new List<RelationshipMeta>(),
                    senders = new List<RelationshipMeta>()
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
        public async Task<ActionResult<Client>> PostSignupAsync([FromBody] NewClientWithResponsesModel clientResponseModel)
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
                    isAdmin = clientResponseModel.isAdmin,
                    hasAccount = clientResponseModel.hasAccount,
                    address = new Logic.Objects.Address()
                    {
                        addressLineOne = clientResponseModel.clientAddressLine1,
                        addressLineTwo = clientResponseModel.clientAddressLine2,
                        city = clientResponseModel.clientCity,
                        state = clientResponseModel.clientState,
                        postalCode = clientResponseModel.clientPostalCode,
                        country = clientResponseModel.clientCountry
                    },
                    assignments = new List<RelationshipMeta>(),
                    senders = new List<RelationshipMeta>()
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
        [HttpPost("{clientID}/Recipients")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PostRecipient(Guid clientID, [FromBody] AddClientRelationshipsModel assignmentsModel)
        {
            try
            {
                foreach(Guid assignmentID in assignmentsModel.assignments)
                {
                    await repository.CreateClientRelationByID(clientID, assignmentID, assignmentsModel.eventTypeID, assignmentsModel.assignmentStatusID);
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
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST: api/Client/AutoAssignments
        /// <summary>
        /// Endpoint for posting selected auto assignment options
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AutoAssignments")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult> PostSelectedAutoAssignments([FromBody] NewAutoAssignmentsModel model)
        {
            try
            {
                Event logicCardEvent = await repository.GetEventByNameAsync(Constants.CARD_EXCHANGE_EVENT);
                AssignmentStatus logicAssignedStatus = (await repository.GetAllAssignmentStatuses()).FirstOrDefault(s => s.assignmentStatusDescription == Constants.ASSIGNED_ASSIGNMENT_STATUS);

                // Gets a list of clients where the sender agents equal the client ID's. These are the people who will recieve status emails
                List<Client> clientsToEmail = (await repository.GetAllClients()).Where(c => model.pairings.Any(p => p.senderAgentID == c.clientID)).ToList();

                foreach(Pairing pair in model.pairings)
                {
                    await repository.CreateClientRelationByID(pair.senderAgentID, pair.assignmentClientID, logicCardEvent.eventTypeID , logicAssignedStatus.assignmentStatusID);
                }
                await repository.SaveAsync();
                foreach(Client massMailer in clientsToEmail)
                {
                    await mailbag.sendAssignedRecipientEmail(massMailer, logicCardEvent);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        // POST: api/Client/5/CreateAccount
        /// <summary>
        /// Endpoint for creating an auth0 account for an existing client
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpPost("{clientID}/CreateAccount")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PostNewAuth0AccountForClientByID(Guid clientID)
        {
            try
            {
                Client logicClient = await repository.GetClientByIDAsync(clientID);
                await Auth0Steps(logicClient, true);

                logicClient.hasAccount = true;
                await repository.UpdateClientByIDAsync(logicClient);
                await repository.SaveAsync();

                return Ok(await repository.GetClientByIDAsync(clientID));
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST: api/Client/5/Tag
        /// <summary>
        /// Assigns a specific user a tag by ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="tagID"></param>
        /// <returns></returns>
        [HttpPost("{clientID}/Tags")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PostClientTagRelationships(Guid clientID, [FromBody] AddClientTagListResponseModel tagsModel)
        {
            try
            {
                foreach(Guid tagID in tagsModel.tags)
                {
                    await repository.CreateClientTagRelationByID(clientID, tagID);
                }
                await repository.SaveAsync();

                Client logicClient = await repository.GetClientByIDAsync(clientID);
                return Ok(logicClient);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // POST: api/Client/5/Password
        /// <summary>
        /// Triggers a password reset email for a user
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpPost("{clientID}/Password")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> sendResetPasswordInformation(Guid clientID)
        {
            try
            {
                Client logicClient = await repository.GetClientByIDAsync(clientID);

                Models.Auth0_Response_Models.Auth0TicketResponse ticket = await authHelper.getPasswordChangeTicketByAuthClientEmail(logicClient.email);
                await mailbag.sendPasswordResetEmail(logicClient.email, logicClient.nickname, ticket, false);

                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
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
        public async Task<ActionResult<Logic.Objects.Client>> PutAddress(Guid clientID, [FromBody] EditClientAddressModel address)
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
                        return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
                    }
                }
                catch(Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
                }
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }
        // PUT: api/Client/5/Email
        /// <summary>
        /// Updates a client's email and Auth0 email, then sends them an option to reset their password
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Email")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutEmail(Guid clientID, [FromBody] EditClientEmailModel email)
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

                    // If the client isn't awaiting or denied (meaning they might have an auth account)
                    if(updatedClient.clientStatus.statusDescription != Constants.AWAITING_STATUS && updatedClient.clientStatus.statusDescription != Constants.DENIED_STATUS)
                    {
                        try
                        {
                            Models.Auth0_Response_Models.Auth0UserInfoModel authClient = new Models.Auth0_Response_Models.Auth0UserInfoModel();

                            // Gets the original client ID by the old email if they have an account
                            if (updatedClient.hasAccount)
                            {
                                authClient = await authHelper.getAuthClientByEmail(oldEmail);
                            }
                            // If the user does not have an account, return the update
                            else if(!updatedClient.hasAccount)
                            {
                                return Ok(updatedClient);
                            }

                            // If the response was null or empty, and the user is marked to have an account, something went wrong with updating their auth0 account. 
                            // Returns a bad request, and sets the email back to the old email in question
                            if (string.IsNullOrEmpty(authClient.user_id) && updatedClient.hasAccount)
                            {
                                targetClient.email = oldEmail;
                                await repository.UpdateClientByIDAsync(targetClient);
                                await repository.SaveAsync();
                                return StatusCode(StatusCodes.Status400BadRequest, "Something went wrong. The user did not have an auth account to update");
                            }
                            // Else if the authclient userId response is not null or empty, and the user is marked to have an account
                            else if(!string.IsNullOrEmpty(authClient.user_id) && updatedClient.hasAccount)
                            {
                                // Updates a client's email in Auth0
                                await authHelper.updateAuthClientEmail(authClient.user_id, updatedClient.email);

                                // Sends the client a password change ticket
                                Models.Auth0_Response_Models.Auth0TicketResponse ticket = await authHelper.getPasswordChangeTicketByAuthClientEmail(updatedClient.email);
                                await mailbag.sendPasswordResetEmail(oldEmail, updatedClient.nickname, ticket, false);
                            }
                        }
                        catch (Exception e)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
                        }
                    }
                    return Ok(updatedClient);
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }
        // PUT: api/Client/5/Nickname
        /// <summary>
        /// Update a client's nickname
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Nickname")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutNickname(Guid clientID, [FromBody] EditClientNicknameModel nickname)
        {
            try
            {
                // Gets client and sets new nickname
                Logic.Objects.Client targetClient = await repository.GetClientByIDAsync(clientID);
                targetClient.nickname = nickname.clientNickname;

                try
                {
                    // Update prep
                    await repository.UpdateClientByIDAsync(targetClient);

                    // If the client does not have an account
                    if (!targetClient.hasAccount)
                    {
                        // Save the changes
                        await repository.SaveAsync();
                    }
                    else
                    {
                        // Set their Auth0 client name to the new nickname and save the repo
                        Models.Auth0_Response_Models.Auth0UserInfoModel authClient = await authHelper.getAuthClientByEmail(targetClient.email);
                        await authHelper.updateAuthClientName(authClient.user_id, nickname.clientNickname);
                        await repository.SaveAsync();
                    }
                    return Ok(await repository.GetClientByIDAsync(targetClient.clientID));
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // PUT: api/Client/5/Name
        /// <summary>
        /// Updates a client's actual name
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Name")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutName(Guid clientID, [FromBody] EditClientNameModel name)
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
                    return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
                }
                
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // PUT: api/Client/5/Admin
        /// <summary>
        /// Updates a client's actual name
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Admin")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<Logic.Objects.Client>> PutIsAdmin(Guid clientID, [FromBody] EditClientIsAdminModel model)
        {
            try
            {
                Logic.Objects.Client targetClient = await repository.GetClientByIDAsync(clientID);
                targetClient.isAdmin = model.isAdmin;
                try
                {
                    await repository.UpdateClientByIDAsync(targetClient);
                    await repository.SaveAsync();
                    Logic.Objects.Client updatedClient = await repository.GetClientByIDAsync(targetClient.clientID);
                    return Ok(updatedClient);
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
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
        public async Task<ActionResult<Logic.Objects.Client>> PutStatus(Guid clientID, [FromBody] EditClientStatusModel status)
        {
            try
            {
                // If the status ID is empty on the request, return 400 bad request
                if(status.clientStatusID.Equals(Guid.Empty))
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                // Grab original client and set the original stattus to its own object for later comparison
                Logic.Objects.Client targetClient = await repository.GetClientByIDAsync(clientID);
                Status originalStatus = targetClient.clientStatus;
                
                try
                {
                    // Updates client status and account status
                    targetClient.clientStatus.statusID = status.clientStatusID;
                    await repository.UpdateClientByIDAsync(targetClient);
                    await repository.SaveAsync();
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }

                // Get updated client
                Logic.Objects.Client updatedClient = await repository.GetClientByIDAsync(targetClient.clientID);
                try
                {
                    // Send approval steps for a client that was awaiting and approved for the event
                    if(updatedClient.clientStatus.statusDescription == Constants.APPROVED_STATUS && originalStatus.statusDescription == Constants.AWAITING_STATUS)
                    {
                        await Auth0Steps(updatedClient, status.wantsAccount);

                        // If approval goes well, and the client wanted an auth0 account, update the hasAccount status to true
                        if(status.wantsAccount)
                        {
                            updatedClient.hasAccount = true;
                            await repository.UpdateClientByIDAsync(updatedClient);
                            await repository.SaveAsync();
                        }
                    }
                    // Send approval steps for client that was denied, and was accepted after appeal
                    else if(updatedClient.clientStatus.statusDescription == Constants.APPROVED_STATUS && originalStatus.statusDescription == Constants.DENIED_STATUS)
                    {
                        await mailbag.sendUndeniedEmail(updatedClient);
                        await Auth0Steps(updatedClient, status.wantsAccount);

                        // If approval goes well, and the client wanted an auth0 account, update the hasAccount status to true
                        if (status.wantsAccount)
                        {
                            updatedClient.hasAccount = true;
                            await repository.UpdateClientByIDAsync(updatedClient);
                            await repository.SaveAsync();
                        }
                    }
                    // Send congrats on completing the gift assignments
                    else if(updatedClient.clientStatus.statusDescription == Constants.COMPLETED_STATUS && originalStatus.statusDescription == Constants.APPROVED_STATUS)
                    {
                        await mailbag.sendCompletedEmail(updatedClient);
                    }
                    // Send re-enlisted email
                    else if (updatedClient.clientStatus.statusDescription == Constants.APPROVED_STATUS && originalStatus.statusDescription == Constants.COMPLETED_STATUS)
                    {
                        await mailbag.sendReelistedEmail(updatedClient);
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
                    targetClient.clientStatus = originalStatus;
                    await repository.UpdateClientByIDAsync(targetClient);
                    await repository.SaveAsync();
                    return StatusCode(StatusCodes.Status417ExpectationFailed, "Something went wrong approving the anon, or sending them an email for the event. Status has been left unchanged.");
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // PUT: api/Client/5/Relationship/5/AssignmentStatus
        /// <summary>
        /// Changes the status of a given assignment by ID's and a body with the new assignment status ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="assignmentRelationshipID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{clientID}/Relationship/{assignmentRelationshipID}/AssignmentStatus")]
        [Authorize(Policy = "update:clients")]
        public async Task<ActionResult<RelationshipMeta>> UpdateRelationshipStatusByID(Guid clientID, Guid assignmentRelationshipID, [FromBody] EditClientAssignmentStatusModel model)
        {
            try
            {
                await repository.UpdateAssignmentProgressStatusByID(assignmentRelationshipID, model.assignmentStatusID);
                await repository.SaveAsync();

                Client logicClient = await repository.GetClientByIDAsync(clientID);
                List<RelationshipMeta> logicMetas = new List<RelationshipMeta>();
                logicMetas.AddRange(logicClient.assignments);
                logicMetas.AddRange(logicClient.senders);

                return (logicMetas.First(r => r.clientRelationXrefID == assignmentRelationshipID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
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
            try
            {
                try
                {
                    Client logicClient = await repository.GetClientByIDAsync(clientID);
                    await repository.DeleteClientByIDAsync(clientID);

                    if (logicClient.clientStatus.statusDescription != Constants.AWAITING_STATUS && logicClient.hasAccount)
                    {
                        Models.Auth0_Response_Models.Auth0UserInfoModel authUser = await authHelper.getAuthClientByEmail(logicClient.email);
                        await authHelper.deleteAuthClient(authUser.user_id);
                    }
                    
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
                }

                await repository.SaveAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
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
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
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
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        /// <summary>
        /// Method for approval steps
        /// </summary>
        /// <param name="logicClient"></param>
        /// <returns></returns>
        private async Task Auth0Steps(Client logicClient, bool wantsAccount)
        {
            if(wantsAccount)
            {
                // Creates auth client
                Models.Auth0_Response_Models.Auth0UserInfoModel authClient = await authHelper.createAuthClient(logicClient.email, logicClient.nickname);

                // Gets all the roles, and grabs the role for participants
                List<Models.Auth0_Response_Models.Auth0RoleModel> roles = await authHelper.getAllAuthRoles();
                Models.Auth0_Response_Models.Auth0RoleModel approvedRole = roles.First(r => r.name == Constants.PARTICIPANT);

                // Updates client with the participant role
                await authHelper.updateAuthClientRole(authClient.user_id, approvedRole.id);

                // Sends the client a password change ticket
                Models.Auth0_Response_Models.Auth0TicketResponse ticket = await authHelper.getPasswordChangeTicketByAuthClientEmail(logicClient.email);
                await mailbag.sendPasswordResetEmail(logicClient.email, logicClient.nickname, ticket, true);
            }
            else
            {
                await mailbag.sendApprovedForEventWithNoAccountEmail(logicClient);
            }
        }
    }
}
