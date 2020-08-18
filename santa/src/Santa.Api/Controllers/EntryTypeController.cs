using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Api.Models.Entry_Type_Models;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryTypeController : ControllerBase
    {
        private readonly IRepository repository;

        public EntryTypeController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        // GET: api/EntryType
        [HttpGet]
        public async Task<ActionResult<List<EntryType>>> GetAllEntryTypes()
        {
            try
            {
                List<EntryType> listLogicEntryTypes = await repository.GetAllEntryTypesAsync();
                return Ok(listLogicEntryTypes);
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // GET: api/EntryType/5
        [HttpGet("{entryTypeID}")]
        public async Task<ActionResult<EntryType>> GetEntryTypeByID(Guid entryTypeID)
        {
            try
            {
                EntryType logicEntryType = await repository.GetEntryTypeByIDAsync(entryTypeID);
                return Ok(logicEntryType);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // POST: api/EntryType
        [HttpPost]
        public async Task<ActionResult<EntryType>> PostNewEntryType([FromBody] NewEntryTypeModel model)
        {
            try
            {
                EntryType logicEntryType = new EntryType()
                {
                    entryTypeID = Guid.NewGuid(),
                    entryTypeName = model.entryTypeName,
                    entryTypeDescription = model.entryTypeDescription
                };
                await repository.CreateEntryTypeAsync(logicEntryType);
                await repository.SaveAsync();

                return Ok(await repository.GetEntryTypeByIDAsync(logicEntryType.entryTypeID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // PUT: api/EntryType/5/Name
        [HttpPut("{entryTypeID}/Name")]
        public async Task<ActionResult<EntryType>> PutEntryTypeName(Guid entryTypeID, [FromBody] EditEntryTypeNameModel model)
        {
            try
            {
                EntryType logicEntryType = new EntryType()
                {
                    entryTypeID = entryTypeID,
                    entryTypeName = model.entryTypeName
                };
                await repository.UpdateEntryTypeName(logicEntryType);
                await repository.SaveAsync();

                return Ok(await repository.GetEntryTypeByIDAsync(entryTypeID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // PUT: api/EntryType/5/Description
        [HttpPut("{entryTypeID}/Description")]
        public async Task<ActionResult<EntryType>> PutEntryTypeDescription(Guid entryTypeID, [FromBody] EditEntryTypeDescriptionModel model)
        {
            try
            {
                EntryType logicEntryType = new EntryType()
                {
                    entryTypeID = entryTypeID,
                    entryTypeDescription = model.entryTypeDescription
                };
                await repository.UpdateEntryTypeDescription(logicEntryType);
                await repository.SaveAsync();

                return Ok(await repository.GetEntryTypeByIDAsync(entryTypeID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }

        // DELETE: api/EntryType/5
        [HttpDelete("{entryTypeID}")]
        public async Task<ActionResult> DeleteEntryType(Guid entryTypeID)
        {
            try
            {
                await repository.DeleteEntryTypeByID(entryTypeID);
                await repository.SaveAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.InnerException);
            }
        }
    }
}
