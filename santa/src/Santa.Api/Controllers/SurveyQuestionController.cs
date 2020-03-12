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
    public class SurveyQuestionController : ControllerBase
    {
        private readonly IRepository repository;
        public SurveyQuestionController(IRepository _repository)
        {
            repository = _repository;
        }

        // GET: api/SurveyQuestion
        /// <summary>
        /// Gets surveyquestion by ID
        /// </summary>
        /// <param name="surveyQuestionID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Logic.Objects.Question>> GetQuestionsAsync()
        {
            try
            {
                List<Logic.Objects.Question> listLogicQuestion = repository.GetAllSurveyQuestions();
                return Ok(listLogicQuestion);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // GET: api/SurveyQuestion/5
        /// <summary>
        /// Gets surveyquestion by ID
        /// </summary>
        /// <param name="surveyQuestionID"></param>
        /// <returns></returns>
        [HttpGet("{surveyQuestionID}")]
        public async Task<ActionResult<Logic.Objects.Question>> GetQuestionsByIDAsync(Guid surveyQuestionID)
        {
            try
            {
                Logic.Objects.Question logicQuestion = await repository.GetSurveyQuestionByIDAsync(surveyQuestionID);
                return Ok(logicQuestion);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // POST: api/SurveyQuestion/5
        /// <summary>
        /// Posts new question. Binds on the ApiQuestion model
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Logic.Objects.Question>> PostSurveyQuestion([FromBody, Bind("questionText, isSurveyOptionList, sortOrder, isActive")] Models.ApiQuestion question)
        {
            try
            {
                Logic.Objects.Question newQuestion = new Logic.Objects.Question()
                {
                    questionID = Guid.NewGuid(),
                    questionText = question.questionText,
                    isSurveyOptionList = question.isSurveyOptionList
                };

                try
                {
                    await repository.CreateSurveyQuestionAsync(newQuestion);
                    await repository.SaveAsync();
                    return Ok(await repository.GetSurveyQuestionByIDAsync(newQuestion.questionID));
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
        //POST: api/SurveyQuestion/5/SurveyOption
        /// <summary>
        /// Posts a new surveyOption for a question given a surveyQuestionID. Binds on the ApiQuestionOption model.
        /// </summary>
        /// <param name="surveyQuestionID"></param>
        /// <param name="questionOption"></param>
        /// <returns></returns>
        [HttpPost("{surveyQuestionID}/SurveyOption")]
        public async Task<ActionResult<Logic.Objects.Option>> PostSurveyQuestionOption(Guid surveyQuestionID, [FromBody, Bind("surveyOptionID, displayText, surveyOptionValue, sortOrder, isActive")] Models.ApiQuestionOption questionOption)
        {
            try
            {
                Logic.Objects.Option logicSurveyOption = new Logic.Objects.Option()
                {
                    surveyOptionID = Guid.NewGuid(),
                    displayText = questionOption.displayText,
                    surveyOptionValue = questionOption.surveyOptionValue
                };
                try
                {
                    await repository.CreateQuestionOptionAsync(logicSurveyOption);
                    await repository.SaveAsync();
                    await repository.CreateSurveyQuestionOptionXrefAsync(logicSurveyOption);
                    await repository.SaveAsync();
                    return Ok(logicSurveyOption);
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

        // PUT: api/SurveyQuestion/5/Text
        /// <summary>
        /// Puts an updated question text to a surveyQuestion given its ID. Binds to the ApiQuestionText model
        /// </summary>
        /// <param name="surveyQuestionID"></param>
        /// <param name="questionText"></param>
        /// <returns></returns>
        [HttpPut("{surveyQuestionID}/Text")]
        public async Task<ActionResult<Logic.Objects.Option>> PutQuestionText(Guid surveyQuestionID, [FromBody, Bind("questionText")] Models.Question_Models.ApiQuestionText questionText)
        {
            try
            {
                Logic.Objects.Question logicQuestion = await repository.GetSurveyQuestionByIDAsync(surveyQuestionID);
                logicQuestion.questionText = questionText.questionText;
                try
                {
                    await repository.UpdateSurveyQuestionByIDAsync(logicQuestion);
                    await repository.SaveAsync();
                    return Ok(await repository.GetSurveyQuestionByIDAsync(surveyQuestionID));
                }
                catch (Exception e)
                {
                    throw e.InnerException;
                }
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        // PUT: api/SurveyQuestion/5/HasOptions
        /// <summary>
        /// Puts an updated true/false type that signified if a survey question is an option list or not. Binds to the ApiQuestionSurveyOptionList model.
        /// </summary>
        /// <param name="surveyQuestionID"></param>
        /// <param name="questionIsSurveyOptionList"></param>
        /// <returns></returns>
        [HttpPut("{surveyQuestionID}/HasOptions")]
        public async Task<ActionResult<Logic.Objects.Option>> PutQuestionIsSurveyOptionList(Guid surveyQuestionID, [FromBody, Bind("isSurveyOptionList")] Models.Question_Models.ApiQuestionSurveyOptionList questionIsSurveyOptionList)
        {
            try
            {
                Logic.Objects.Question logicQuestion = await repository.GetSurveyQuestionByIDAsync(surveyQuestionID);
                logicQuestion.isSurveyOptionList = questionIsSurveyOptionList.isSurveyOptionList;
                try
                {
                    await repository.UpdateSurveyQuestionByIDAsync(logicQuestion);
                    await repository.SaveAsync();
                    return Ok(await repository.GetSurveyQuestionByIDAsync(surveyQuestionID));
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

        // DELETE: api/SurveyQuestion/5
        /// <summary>
        /// Deletes an existing survey question by its ID.
        /// </summary>
        /// <param name="surveyQuestionID"></param>
        /// <returns></returns>
        [HttpDelete("{surveyQuestionID}")]
        public async Task<ActionResult> Delete(Guid surveyQuestionID)
        {
            try
            {
                await repository.DeleteSurveyQuestionByIDAsync(surveyQuestionID);
                await repository.SaveAsync();
                return NoContent();
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
