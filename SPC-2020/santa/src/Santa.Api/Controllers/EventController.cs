using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Api.Models.Event_Models;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [AllowAnonymous]
        public async Task<ActionResult<List<Logic.Objects.Event>>> GetAllEvents()
        {
            try
            {
                return Ok(await repository.GetAllEvents());
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
        [AllowAnonymous]
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
        [Authorize(Policy = "create:events")]
        public async Task<ActionResult<Logic.Objects.Event>> Post([FromBody]Models.ApiEvent newEvent)
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
        [HttpPut("{eventID}/Description")]
        [Authorize(Policy = "update:events")]
        public async Task<ActionResult<Logic.Objects.Event>> PutDescription(Guid eventID, [FromBody] ApiEventDescription description)
        {
            try
            {
                Logic.Objects.Event targetEvent = await repository.GetEventByIDAsync(eventID);
                targetEvent.eventDescription = description.eventDescription;
                try
                {
                    await repository.UpdateEventByIDAsync(targetEvent);
                    await repository.SaveAsync();
                    return Ok(await repository.GetEventByIDAsync(eventID));
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
        // PUT: api/Event/5
        [HttpPut("{eventID}/Active")]
        [Authorize(Policy = "update:events")]
        public async Task<ActionResult<Logic.Objects.Event>> PutDescription(Guid eventID, [FromBody] ApiEventActive active)
        {
            try
            {
                Logic.Objects.Event targetEvent = await repository.GetEventByIDAsync(eventID);
                targetEvent.active = active.eventIsActive;
                try
                {
                    await repository.UpdateEventByIDAsync(targetEvent);
                    await repository.SaveAsync();
                    return Ok(await repository.GetEventByIDAsync(eventID));
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

        // DELETE: api/Event/5
        [HttpDelete("{eventID}")]
        [Authorize(Policy = "delete:events")]
        public async Task<ActionResult> Delete(Guid eventID)
        {
            try
            {
                await repository.DeleteEventByIDAsync(eventID);
                await repository.SaveAsync();
                return NoContent();
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
