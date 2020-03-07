using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Santa.Api.Models;
using Santa.Logic.Interfaces;

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
                Logic.Objects.Client client = await repository.GetClientByID(clientID);
                return Ok(JsonConvert.SerializeObject(client, Formatting.Indented));
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
                    }
                };
                
                try
                {
                    await repository.CreateClient(newClient);
                    await repository.SaveAsync();
                    return Created($"api/Client/{newClient.clientID}", newClient);
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

        // PUT: api/Client/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
