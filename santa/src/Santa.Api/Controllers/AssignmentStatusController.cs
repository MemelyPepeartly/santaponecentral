using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Api.Models.Assignment_Status_Models;
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
        /// <summary>
        /// Gets a list of all assignment statuses
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Gets a specific assignment status by ID
        /// </summary>
        /// <param name="assignmentStatusID"></param>
        /// <returns></returns>
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

        // POST: api/AssignmentStatus
        /// <summary>
        /// Creates a new assignment status
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "create:assignmentStatuses")]
        public async Task<ActionResult<Logic.Objects.AssignmentStatus>> PostNewAssignmentStatus([FromBody] NewAssignmentStatusModel model)
        {
            try
            {
                AssignmentStatus newLogicAssignmentStatus = new AssignmentStatus()
                {
                    assignmentStatusID = Guid.NewGuid(),
                    assignmentStatusName = model.assignmentStatusName,
                    assignmentStatusDescription = model.assignmentStatusDescription
                };
                await repository.CreateAssignmentStatus(newLogicAssignmentStatus);
                await repository.SaveAsync();

                AssignmentStatus newAssignmentStatus = await repository.GetAssignmentStatusByID(newLogicAssignmentStatus.assignmentStatusID);
                return Ok(newAssignmentStatus);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT: api/AssignmentStatus/5
        /// <summary>
        /// Edits the details of an assignment status
        /// </summary>
        /// <param name="assignmentStatusID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{assignmentStatusID}")]
        [Authorize(Policy = "update:assignmentStatuses")]
        public async Task<ActionResult<Logic.Objects.AssignmentStatus>> PutAssignmentStatus(Guid assignmentStatusID, [FromBody] UpdateAssignmentStatusModel model)
        {
            try
            {
                AssignmentStatus targetAssignmentStatus = new AssignmentStatus()
                {
                    assignmentStatusID = assignmentStatusID,
                    assignmentStatusName = model.assignmentStatusName,
                    assignmentStatusDescription = model.assignmentStatusDescription
                };
                await repository.UpdateAssignmentStatus(targetAssignmentStatus);
                await repository.SaveAsync();

                AssignmentStatus updatedAssignmenStatus = await repository.GetAssignmentStatusByID(assignmentStatusID);
                return Ok(updatedAssignmenStatus);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Deletes an assignment status by ID
        /// </summary>
        /// <param name="assignmentStatusID"></param>
        /// <returns></returns>
        [HttpDelete("{assignmentStatusID}")]
        [Authorize(Policy = "delete:assignmentStatuses")]
        public async Task<ActionResult> DeleteAssignmentStatusByID(Guid assignmentStatusID)
        {
            try
            {
                await repository.DeleteAssignmentStatusByID(assignmentStatusID);
                await repository.SaveAsync();

                return NoContent();
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
