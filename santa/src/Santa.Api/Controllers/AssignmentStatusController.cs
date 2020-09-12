using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AssignmentStatusController : ControllerBase
    {
        private readonly IRepository repository;

        public AssignmentStatusController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        // GET: api/AssignmentStatus
        [HttpGet]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<List<Logic.Objects.AssignmentStatus>>> GetAllAssignmentStatuses()
        {
            try
            {
                List<Logic.Objects.AssignmentStatus> logicListAssignmentStatuses = await repository.GetAllAssignmentStatuses();
                return Ok(logicListAssignmentStatuses);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/AssignmentStatus/5
        [HttpGet("{assignmentStatusID}")]
        [Authorize(Policy = "read:profile")]
        public async Task<ActionResult<Logic.Objects.AssignmentStatus>> GetAssignmentStatusByID(Guid assignmentStatusID)
        {
            try
            {
                Logic.Objects.AssignmentStatus logicAssignmentStatus = await repository.GetAssignmentStatusByID(assignmentStatusID);
                return Ok(logicAssignmentStatus);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        // GET: api/AssignmentStatus/GetAll/5
        [HttpGet("GetAll/{assignmentStatusID}")]
        [Authorize(Policy = "read:clients")]
        public async Task<ActionResult<List<Logic.Objects.AssignmentStatus>>> GetAllAssignmentsWithStatusByID(Guid assignmentStatusID)
        {
            try
            {
                List<object> listLogicAssignmentStatuses = await repository.GetAssignmentsByAssignmentStatusID(assignmentStatusID);
                return Ok(listLogicAssignmentStatuses);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST: api/AssignmentStatus
        [HttpPost]
        [Authorize(Policy = "create:assignmentStatuses")]
        public async Task<ActionResult<Logic.Objects.AssignmentStatus>> PostNewAssignmentStatus([FromBody] string value)
        {
            try
            {
                AssignmentStatus targetAssignmentStatus = new AssignmentStatus();
                await repository.UpdateAssignmentStatus(targetAssignmentStatus);
                AssignmentStatus newAssignmentStatus = await repository.GetAssignmentStatusByID(targetAssignmentStatus.assignmentStatusID);

                return Ok(newAssignmentStatus);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT: api/AssignmentStatus/5
        [HttpPut("{assignmentStatusID}")]
        [Authorize(Policy = "update:assignmentStatuses")]
        public async Task<ActionResult<Logic.Objects.AssignmentStatus>> PutAssignmentStatus(Guid assignmentStatusID, [FromBody] string value)
        {
            try
            {
                AssignmentStatus targetAssignmentStatus = new AssignmentStatus();
                await repository.UpdateAssignmentStatus(targetAssignmentStatus);
                AssignmentStatus updatedAssignmenStatus = await repository.GetAssignmentStatusByID(assignmentStatusID);

                return Ok(updatedAssignmenStatus);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{assignmentStatusID}")]
        [Authorize(Policy = "delete:assignmentStatuses")]
        public async Task<ActionResult> DeleteAssignmentStatusByID(Guid assignmentStatusID)
        {
            try
            {
                await repository.DeleteAssignmentStatusByID(assignmentStatusID);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
