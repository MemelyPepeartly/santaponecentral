using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Logic.Interfaces;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IRepository repository;
        public EventController(IRepository _repository)
        {
            repository = _repository;
        }
        // GET: api/Event
        /// <summary>
        /// Gets list of events
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Logic.Objects.Event>> GetAllEvents()
        {
            try
            {
                return Ok(repository.GetAllEvents());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Event/5
        /// <summary>
        /// Gets event by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{eventID}")]
        public async Task<ActionResult<Logic.Objects.Event>> GetEventByID(Guid eventID)
        {
            try
            {
                return Ok(await repository.GetEventByIDAsync(eventID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST: api/Event
        [HttpPost]
        public async Task<ActionResult<Logic.Objects.Event>> Post([FromBody, Bind("eventDescription, isActive")]Models.ApiEvent newEvent)
        {
            try
            {
                Logic.Objects.Event logicEvent = new Logic.Objects.Event()
                {
                    eventTypeID = Guid.NewGuid(),
                    eventDescription = newEvent.eventDescription,
                    active = newEvent.isActive
                };
                try
                {
                    await repository.CreateEventAsync(logicEvent);
                    await repository.SaveAsync();
                    return Ok(await repository.GetEventByIDAsync(logicEvent.eventTypeID));
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

        // PUT: api/Event/5
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
