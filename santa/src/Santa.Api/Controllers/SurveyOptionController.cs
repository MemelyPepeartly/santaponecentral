using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Logic.Interfaces;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyOptionController : ControllerBase
    {
        private readonly IRepository repository;
        public SurveyOptionController(IRepository _repository)
        {
            repository = _repository;
        }
        // GET: api/SurveyOption
        [HttpGet]
        public ActionResult<List<Logic.Objects.Option>> GetAllSurveyOptions()
        {
            try
            {
                return Ok(repository.GetAllSurveyOption());
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        // GET: api/SurveyOption/5
        [HttpGet("{surveyOptionID}")]
        public async Task<ActionResult<Logic.Objects.Option>> GetSurveyOptionByIDAsync(Guid surveyOptionID)
        {
            try
            {
                return Ok(await repository.GetSurveyOptionByIDAsync(surveyOptionID));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // PUT: api/SurveyOption/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            try
            {

            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            try
            {

            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
