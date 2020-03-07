﻿using System;
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
        [HttpGet("{id}", Name = "Get")]
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
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Status/5
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
