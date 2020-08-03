using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Santa.Api.Models;
using Santa.Logic.Interfaces;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SurveyController : ControllerBase
    {

        private readonly IRepository repository;
        public SurveyController(IRepository _repository)
        {
            repository = _repository;
        }

        // GET: api/Survey
        /// <summary>
        /// Gets list of surveys
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Logic.Objects.Survey>>> GetAllSurveys()
        {
            try
            {
                return Ok(await repository.GetAllSurveys());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Survey/5
        /// <summary>
        /// Gets survey by ID
        /// </summary>
        /// <param name="surveyID"></param>
        /// <returns></returns>
        [HttpGet("{surveyID}")]
        [AllowAnonymous]
        public async Task<ActionResult<Logic.Objects.Survey>> GetSurveyByIDAsync(Guid surveyID)
        {
            try
            {
                return Ok(await repository.GetSurveyByID(surveyID));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        // GET: api/Survey/5/SurveyQuestions
        /// <summary>
        /// Gets surveyquestions within a given survey by surveyID
        /// </summary>
        /// <param name="surveyID"></param>
        /// <returns></returns>
        [HttpGet("{surveyID}/SurveyQuestions")]
        [AllowAnonymous]
        public async Task<ActionResult<Logic.Objects.Survey>> GetQuestionsFromSurveyAsync(Guid surveyID)
        {
            try
            {
                Logic.Objects.Survey logicSurvey = await repository.GetSurveyByID(surveyID);
                return Ok(logicSurvey.surveyQuestions);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        //GET: api/Survey/5/SurveyQuestions/5/SurveyOptions
        /// <summary>
        /// Gets a list of question options from a question within a given survey
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="surveyQuestionID"></param>
        /// <returns></returns>
        [HttpGet("{surveyID}/SurveyQuestions/{surveyQuestionID}/SurveyOptions")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Logic.Objects.Option>>> GetQuestionOptionFromQuestionInSurveyAsync(Guid surveyID, Guid surveyQuestionID)
        {
            try
            {
                Logic.Objects.Survey logicSurvey = await repository.GetSurveyByID(surveyID);
                Logic.Objects.Question logicQuestion = logicSurvey.surveyQuestions.FirstOrDefault(q => q.questionID == surveyQuestionID);
                return Ok(logicQuestion.surveyOptionList);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // POST: api/Survey
        /// <summary>
        /// Posts new survey. Binds on the ApiSurvey model
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy ="create:surveys")]
        public async Task<ActionResult<ApiSurvey>> PostSurvey([FromBody, Bind("eventTypeID, surveyDescription, active")] Api.Models.ApiSurvey survey)
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

                try
                {
                    await repository.CreateSurveyAsync(newSurvey);
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

        // PUT: api/Survey/5
        /// <summary>
        /// Put updates a survey's description by survey ID. Binds to ApiSurveyDescription model.
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [HttpPut("{surveyID}/Description")]
        [Authorize(Policy = "update:surveys")]
        public async Task<ActionResult<Logic.Objects.Survey>> PutSurveyDescription(Guid surveyID, [FromBody, Bind("surveyDescription")] Models.Survey_Models.ApiSurveyDescription description)
        {
            try
            {
                Logic.Objects.Survey targetSurvey = await repository.GetSurveyByID(surveyID);
                targetSurvey.surveyDescription = description.surveyDescription;
                try
                {
                    await repository.UpdateSurveyByIDAsync(targetSurvey);
                    await repository.SaveAsync();
                    return Ok(await repository.GetSurveyByID(surveyID));
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

        // PUT: api/Survey/5
        /// <summary>
        /// Put updates a survey's active status. Binds to the ApiSurveyActive model.
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        [HttpPut("{surveyID}/Active")]
        [Authorize(Policy = "update:surveys")]
        public async Task<ActionResult<Logic.Objects.Survey>> PutSurveyActive(Guid surveyID, [FromBody, Bind("isActive")] Models.Survey_Models.ApiSurveyActive active)
        {
            try
            {
                Logic.Objects.Survey targetSurvey = await repository.GetSurveyByID(surveyID);
                targetSurvey.active = active.isActive;
                try
                {
                    await repository.UpdateSurveyByIDAsync(targetSurvey);
                    await repository.SaveAsync();
                    return Ok(await repository.GetSurveyByID(surveyID));
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

        // DELETE: api/Survey/5
        /// <summary>
        /// Delete's a question within a survey by deleting it's Xref relationship.
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="surveyQuestionID"></param>
        /// <returns></returns>
        [HttpDelete("{surveyID}/SurveyQuestions/{surveyQuestionID}")]
        [Authorize(Policy = "delete:surveys")]
        public async Task<ActionResult<Logic.Objects.Survey>> DeleteSurveyQuestionRelation(Guid surveyID, Guid surveyQuestionID)
        {
            try
            {
                await repository.DeleteSurveyQuestionXrefBySurveyIDAndQuestionID(surveyID, surveyQuestionID);
                await repository.SaveAsync();
                return Ok(await repository.GetSurveyByID(surveyID));
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        // DELETE: api/Survey/5
        /// <summary>
        /// Deletes a survey by its surveyID
        /// </summary>
        /// <param name="surveyID"></param>
        /// <returns></returns>
        [HttpDelete("{surveyID}")]
        [Authorize(Policy = "delete:surveys")]
        public async Task<ActionResult> DeleteSurvey(Guid surveyID)
        {
            try
            {
                await repository.DeleteSurveyByIDAsync(surveyID);
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
