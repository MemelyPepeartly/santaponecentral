using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            repository = _repository;
        }
        // GET: api/Client
        [HttpGet]
        public List<Logic.Objects.Client> Get()
        {
            return repository.GetAllClients();
        }

        // GET: api/Client/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Client
        [HttpPost("new", Name = "NewClient")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ApiClient> Post([FromBody] ApiClient client)
        {
            try
            {
                var newClient = new Logic.Objects.Client()
                {
                    clientID = client.clientID.GetValueOrDefault(),
                    clientName = client.clientName,
                    email = client.email,
                    nickname = client.nickname,
                    address = new Logic.Objects.Address()
                    {
                        addressLineOne = client.addressLineOne,
                        addressLineTwo = client.addressLineTwo,
                        city = client.city,
                        state = client.state,
                        postalCode = client.postalCode,
                        country = client.country
                    }
                };


                return Created($"api/Client/{newClient.clientID}", newClient);
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
