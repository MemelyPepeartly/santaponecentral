﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Santa.Api.Models;
using Santa.Api.Models.Client_Models;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private readonly IRepository repository;
        public ClientController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }
        // GET: api/Client
        /// <summary>
        /// Gets all clients
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Logic.Objects.Client>> GetAllClients()
        {
            try
            {
                List<Logic.Objects.Client> clients = repository.GetAllClients();
                if (clients == null)
                {
                    return NotFound();
                }
                return Ok(JsonConvert.SerializeObject(clients, Formatting.Indented));
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
        public async Task<ActionResult<Logic.Objects.Client>> GetClientByIDAsync(Guid clientID)
        {
            try
            {
                Logic.Objects.Client client = await repository.GetClientByIDAsync(clientID);
                return Ok(JsonConvert.SerializeObject(client, Formatting.Indented));
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
        public ActionResult<List<Logic.Objects.Client>> GetClientResponsesByIDAsync(Guid clientID)
        {
            try
            {
                return Ok(repository.GetAllSurveyResponsesByClientID(clientID));
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        // POST: api/Client/5/Relationship
        [HttpPost("{clientID}/Relationship", Name = "PostRelationship")]
        public async Task<ActionResult<Logic.Objects.Client>> PostRelationship(Guid clientID, [FromBody, Bind("clientNickname")] ApiClientRelationship relationship)
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

        // PUT: api/Client/5/Address
        [HttpPut("{clientID}/Address")]
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
        public async Task<ActionResult<Logic.Objects.Client>> PutEmail(Guid clientID, [FromBody, Bind("clientEmail")] ApiClientEmail email)
        {
            try
            {
                Logic.Objects.Client targetClient = await repository.GetClientByIDAsync(clientID);
                targetClient.email = email.clientEmail;

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
        // PUT: api/Client/5/Nickname
        [HttpPut("{clientID}/Nickname", Name = "PutNickname")]
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
        // PUT: api/Client/5/Name
        [HttpPut("{clientID}/Status", Name = "PutStatus")]
        public async Task<ActionResult<Logic.Objects.Client>> PutStatus(Guid clientID, [FromBody, Bind("clientStatusID")] ApiClientStatus status)
        {
            try
            {
                Logic.Objects.Client targetClient = await repository.GetClientByIDAsync(clientID);
                targetClient.clientStatus.statusID = status.clientStatusID;
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
        // DELETE: api/Client/5
        [HttpDelete("{clientID}")]
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
        // DELETE: api/Client/5
        [HttpDelete("{clientID}/Recipient")]
        public async Task<ActionResult<Logic.Objects.Client>> DeleteRecipientXref(Guid clientID, Guid recipientID)
        {
            try
            {
                await repository.DeleteRecieverXref(clientID, recipientID);
                await repository.SaveAsync();
                return (await repository.GetClientByIDAsync(clientID));
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
