using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Santa.Api.Models;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StatusController : ControllerBase
    {
        private readonly IRepository repository;
        public StatusController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }
        // GET: api/Status
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Logic.Objects.Status>>> GetAllClientStatus()
        {
            try
            {
                return Ok(await repository.GetAllClientStatus());
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // GET: api/Status/5
        [HttpGet("{clientStatusID}")]
        [AllowAnonymous]
        public async Task<ActionResult<Logic.Objects.Status>> GetClientStatusByID(Guid clientStatusID)
        {
            try
            {
                return Ok(await repository.GetClientStatusByID(clientStatusID));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        // GET: api/Status/Check/email
        [HttpGet("Check/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult<Logic.Objects.Status>> GetClientStatusByEmail(string email)
        {
            try
            {
                return Ok((await repository.GetClientByEmailAsync(email)).clientStatus);
            }
            catch(Exception e)
            {
                return Ok(new Status()
                {
                    statusDescription = "Not Found"
                });
            }
        }


        // POST: api/Status
        [HttpPost]
        [Authorize(Policy = "create:statuses")]
        public async Task<ActionResult<Logic.Objects.Status>> PostStatus([FromBody, Bind("statusDescription")] Models.ApiStatusDescription clientStatus)
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
                    throw e.InnerException;
                }
                
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        // PUT: api/Status/5
        [HttpPut("{clientStatusID}")]
        [Authorize(Policy = "modify:statuses")]

        public async Task<ActionResult<Logic.Objects.Status>> Put(Guid clientStatusID, [FromBody, Bind("statusDescription")] Models.ApiStatusDescription changedStatus)
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
                    throw e.InnerException;
                }

            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // DELETE: api/Status/5
        [HttpDelete("{clientStatusID}")]
        [Authorize(Policy = "delete:statuses")]
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
                throw e.InnerException;
            }
        }
    }
}
