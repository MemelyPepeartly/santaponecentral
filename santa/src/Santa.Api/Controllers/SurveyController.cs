using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Api.Models;
using Santa.Logic.Interfaces;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly IRepository repository;
        public SurveyController(IRepository _repository)
        {
            repository = _repository;
        }
        // GET: api/Survey
        [HttpGet]
        public ActionResult<List<Logic.Objects.Survey>> Get()
        {
            try
            {
                return Ok(repository.GetAllSurveys());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Survey/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Logic.Objects.Survey>> GetAsync(Guid id)
        {
            try
            {
                return Ok(await repository.GetSurveyByID(id));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Survey/5/SurveyQuestions
        [HttpGet("{id}/SurveyQuestions")]
        public async Task<ActionResult<Logic.Objects.Survey>> GetQuestionsAsync(Guid id)
        {
            try
            {
                return Ok(await repository.GetSurveyByID(id));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // POST: api/Survey
        [HttpPost]
        public async Task<ActionResult<ApiSurvey>> Post([FromBody, Bind("eventTypeID, surveyDescription, active")] Api.Models.ApiSurvey survey)
        {
            try
            {
                Logic.Objects.Survey newSurvey = new Logic.Objects.Survey()
                {
                    surveyID = Guid.NewGuid(),
                    eventTypeID = survey.eventTypeID,
                    surveyDescription = survey.surveyDescription,
                    active = survey.active
                };
                repository.CreateSurvey(newSurvey);
                try
                {
                    await repository.SaveAsync();
                    return Created($"api/Survey/{newSurvey.surveyID}", newSurvey);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpPost("{id}/SurveyQuestions")]
        public async Task<ActionResult<Logic.Objects.Question>> PostSurveyQuestions(Guid id,[FromBody, Bind("questionText, isSurveyOptionList")] Logic.Objects.Question question)
        {
            try
            {
                Logic.Objects.Question newQuestion = question;
                question.questionID = Guid.NewGuid();

                try
                {
                    await repository.SaveAsync();
                    return Created($"api/Survey/{id}", id);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT: api/Survey/5
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
