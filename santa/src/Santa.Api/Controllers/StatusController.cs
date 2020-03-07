using System;
using System.Collections.Generic;
using System.Linq;
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
    public class StatusController : ControllerBase
    {
        private readonly IRepository repository;
        public StatusController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }
        // GET: api/Status
        [HttpGet]
        public ActionResult<List<Logic.Objects.Status>> GetAllClientStatus()
        {
            try
            {
                return Ok(repository.GetAllClientStatus());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // GET: api/Status/5
        [HttpGet("{clientStatusID}")]
        public async Task<ActionResult<Logic.Objects.Status>> GetClientStatusByID(Guid clientStatusID)
        {
            try
            {
                return Ok(await repository.GetClientStatusByID(clientStatusID));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // POST: api/Status
        [HttpPost]
        public async Task<ActionResult<Logic.Objects.Status>> PostStatus([FromBody, Bind("statusDescription")] Models.ApiClientStatus clientStatus)
        {
            try
            {
                Logic.Objects.Status newStatus = new Logic.Objects.Status()
                {
                    statusID = Guid.NewGuid(),
                    statusDescription = clientStatus.statusDescription
                };
                try
                {
                    await repository.CreateStatusAsync(newStatus);
                    await repository.SaveAsync();
                    return Ok(newStatus);
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message);
                }
                
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // PUT: api/Status/5
        [HttpPut("{clientStatusID}")]
        public async Task<ActionResult<Logic.Objects.Status>> Put(Guid clientStatusID, [FromBody, Bind("statusDescription")] Models.ApiClientStatus changedStatus)
        {
            try
            {
                try
                {
                    Logic.Objects.Status targetStatus = await repository.GetClientStatusByID(clientStatusID);
                    targetStatus.statusDescription = changedStatus.statusDescription;

                    await repository.UpdateStatusByIDAsync(targetStatus);
                    await repository.SaveAsync();
                    return Ok(await repository.GetClientStatusByID(clientStatusID));
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{clientStatusID}")]
        public async Task<ActionResult<Logic.Objects.Status>> Delete(Guid clientStatusID)
        {
            try
            {
                await repository.DeleteStatusByIDAsync(clientStatusID);
                await repository.SaveAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
