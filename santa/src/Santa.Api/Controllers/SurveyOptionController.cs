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

        // PUT: api/SurveyOption/5/DisplayText
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

        // DELETE: api/ApiWithActions/5
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
