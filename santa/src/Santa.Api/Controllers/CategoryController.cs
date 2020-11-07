﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects.Base_Objects.Logging;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository repository;

        public CategoryController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<List<Logic.Objects.Client>>> GetAllCategories()
        {
            return Ok();
        }

        // GET: api/Category/5
        [HttpGet("{categoryID}")]
        public async Task<ActionResult<Category>> GetCategoryByID(Guid categoryID)
        {
            return Ok();
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<Category>> PostNewCategory([FromBody] object model)
        {
            return Ok();
        }

        // PUT: api/Category/5
        [HttpPut("{categoryID}")]
        public async Task<ActionResult<Category>> PutCategory(Guid categoryID, [FromBody] object model)
        {
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{categoryID}")]
        public async Task<ActionResult> DeleteByID(Guid categoryID)
        {
            return NoContent();
        }
    }
}
