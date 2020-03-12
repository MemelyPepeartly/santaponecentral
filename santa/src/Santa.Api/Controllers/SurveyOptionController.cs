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
        /// <summary>
        /// Gets a list of all survey options
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Gets survey option by a given surveyOptionID
        /// </summary>
        /// <param name="surveyOptionID"></param>
        /// <returns></returns>
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

        public async Task<ActionResult<Logic.Objects.Option>> PostSurveyOption([FromBody, Bind("")] Models.ApiSurveyOption newSurveyOption)
        {
            try
            {
                Logic.Objects.Option logicOption = new Logic.Objects.Option()
                {
                    surveyOptionID = Guid.NewGuid(),
                    displayText = newSurveyOption.displayText,
                    surveyOptionValue = newSurveyOption.surveyOptionValue
                };
                try
                {
                    await repository.CreateSurveyOptionAsync(logicOption);
                    await repository.SaveAsync();
                    return Ok(await repository.GetSurveyOptionByIDAsync(logicOption.surveyOptionID));
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

        // PUT: api/SurveyOption/5/DisplayText
        /// <summary>
        /// Puts a new display text for a survey option by surveyOptionID. Binds to the ApiSurveyOptionDisplayText model.
        /// </summary>
        /// <param name="surveyOptionID"></param>
        /// <param name="displayText"></param>
        /// <returns></returns>
        [HttpPut("{surveyOptionID}/DisplayText")]
        public async Task<ActionResult<Logic.Objects.Option>> PutDisplayText(Guid surveyOptionID, [FromBody, Bind("surveyOptionDisplayText")] Models.Survey_Option_Models.ApiSurveyOptionDisplayText displayText)
        {
            try
            {
                Logic.Objects.Option logicOption = await repository.GetSurveyOptionByIDAsync(surveyOptionID);
                logicOption.displayText = displayText.surveyOptionDisplayText;
                try
                {
                    await repository.UpdateSurveyOptionByIDAsync(logicOption);
                    await repository.SaveAsync();
                    return Ok(await repository.GetSurveyOptionByIDAsync(surveyOptionID));
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
        
        // PUT: api/SurveyOption/5/Value
        /// <summary>
        /// Puts a new value for a survey option by surveyOptionID. Binds to the ApiSurveyOptionValue model.
        /// </summary>
        /// <param name="surveyOptionID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{surveyOptionID}/Value")]
        public async Task<ActionResult<Logic.Objects.Option>> PutValue(Guid surveyOptionID, [FromBody, Bind("surveyOptionValue")] Models.Survey_Option_Models.ApiSurveyOptionValue value)
        {
            try
            {
                Logic.Objects.Option logicOption = await repository.GetSurveyOptionByIDAsync(surveyOptionID);
                logicOption.surveyOptionValue = value.surveyOptionValue;
                try
                {
                    await repository.UpdateSurveyOptionByIDAsync(logicOption);
                    await repository.SaveAsync();
                    return Ok(await repository.GetSurveyOptionByIDAsync(surveyOptionID));
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

        // DELETE: api/SurveyOption/5
        /// <summary>
        /// Deletes a survey option by a given surveyOptionID.
        /// </summary>
        /// <param name="surveyOptionID"></param>
        /// <returns></returns>
        [HttpDelete("{surveyOptionID}")]
        public async Task<ActionResult> Delete(Guid surveyOptionID)
        {
            try
            {
                await repository.DeleteSurveyOptionByIDAsync(surveyOptionID);
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
