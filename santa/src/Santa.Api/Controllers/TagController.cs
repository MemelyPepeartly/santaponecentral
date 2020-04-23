using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Santa.Logic.Interfaces;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IRepository repository;
        public TagController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }
        // GET: api/Tag
        [HttpGet]
        public ActionResult<List<Logic.Objects.Tag>> GetAllTags()
        {
            try
            {
                List<Logic.Objects.Tag> logicTags = repository.GetAllTags();
                if (logicTags == null)
                {
                    return NotFound();
                }
                return Ok(logicTags);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Tag/5
        [HttpGet("{id}", Name = "Get Tag")]
        public async ActionResult<Logic.Objects.Tag> GetTagByID(Guid tagID)
        {
            try
            {
                return Ok(repository.GetTagByIDAsync(tagID));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // POST: api/Tag
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Tag/5
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
