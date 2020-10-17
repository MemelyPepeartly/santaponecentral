using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Santa.Api.Models.Tag_Models;
using Santa.Logic.Interfaces;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class TagController : ControllerBase
    {
        private readonly IRepository repository;
        public TagController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        // GET: api/Tag
        [HttpGet]
        [Authorize(Policy = "read:tags")]
        public async Task<ActionResult<List<Logic.Objects.Tag>>> GetAllTags()
        {
            List<Logic.Objects.Tag> logicTags = await repository.GetAllTags();
            if (logicTags == null)
            {
                return NotFound();
            }
            return Ok(logicTags);
        }

        // GET: api/Tag/5
        [HttpGet("{tagID}")]
        [Authorize(Policy = "read:tags")]
        public async Task<ActionResult<Logic.Objects.Tag>> GetTagByID(Guid tagID)
        {
            return Ok(await repository.GetTagByIDAsync(tagID));
        }

        // POST: api/Tag
        [HttpPost]
        [Authorize(Policy = "create:tags")]
        public async Task<ActionResult<Logic.Objects.Tag>> PostTag([FromBody, Bind("tagName")] ApiTag tag)
        {
            Logic.Objects.Tag newLogicTag = new Logic.Objects.Tag()
            {
                tagID = Guid.NewGuid(),
                tagName = tag.tagName
            };
            await repository.CreateTag(newLogicTag);
            await repository.SaveAsync();
            return Ok(await repository.GetTagByIDAsync(newLogicTag.tagID));
        }

        // PUT: api/Tag/5
        [HttpPut("{tagID}")]
        [Authorize(Policy = "update:tags")]
        public async Task<ActionResult<Logic.Objects.Tag>> PutTagName(Guid tagID, [FromBody, Bind("tagName")] ApiTag tag)
        {
            Logic.Objects.Tag logicTag = await repository.GetTagByIDAsync(tagID);
            logicTag.tagName = tag.tagName;

            await repository.UpdateTagNameByIDAsync(logicTag);
            await repository.SaveAsync();
            return (await repository.GetTagByIDAsync(tagID));
        }

        // DELETE: api/Tag/5
        [HttpDelete("{tagID}")]
        [Authorize(Policy = "delete:tags")]
        public async Task<ActionResult> DeleteTag(Guid tagID)
        {
            await repository.DeleteTagByIDAsync(tagID);
            await repository.SaveAsync();
            return NoContent();
        }
    }
}
