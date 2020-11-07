using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using System.Security.Claims;
using Santa.Api.Services.YuleLog;
using Santa.Logic.Constants;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SurveyResponseController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IYuleLog yuleLogger;

        public SurveyResponseController(IRepository _repository, IYuleLog _yuleLogger)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            yuleLogger = _yuleLogger ?? throw new ArgumentNullException(nameof(_yuleLogger));
        }

        // GET: api/SurveyResponses
        [HttpGet]
        [Authorize(Policy = "read:responses")]
        public async Task<ActionResult<List<Logic.Objects.Response>>> GetSurveyResponse()
        {
            return Ok(await repository.GetAllSurveyResponses());
        }
        
        // GET: api/SurveyResponse/5
        [HttpGet("{surveyResponseID}")]
        [Authorize(Policy = "read:responses")]
        public async Task<ActionResult<Response>> GetSurveyResponse(Guid surveyResponseID)
        {
            Logic.Objects.Response surveyResponse = await repository.GetSurveyResponseByIDAsync(surveyResponseID);

            if (surveyResponse == null)
            {
                return NotFound();
            }

            return Ok(surveyResponse);
        }

        // PUT: api/SurveyResponse/5
        /// <summary>
        /// Updates a response by ID
        /// </summary>
        /// <param name="surveyResponseID"></param>
        /// <param name="responseText"></param>
        /// <returns></returns>
        [HttpPut("{surveyResponseID}/ResponseText")]
        [Authorize(Policy = "update:responses")]
        public async Task<ActionResult<Logic.Objects.Response>> PutSurveyResponse(Guid surveyResponseID, Models.Survey_Response_Models.ApiSurveyReponseText responseText)
        {
            Response logicResponse = await repository.GetSurveyResponseByIDAsync(surveyResponseID);
            Client checkerClient = await repository.GetClientByEmailAsync(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            if(checkerClient.isAdmin || logicResponse.clientID == checkerClient.clientID)
            {
                Response logicOldSurveyResponse = await repository.GetSurveyResponseByIDAsync(surveyResponseID);
                Response logicNewSurveyResponse = logicOldSurveyResponse;
                logicNewSurveyResponse.responseText = responseText.responseText;
                await repository.UpdateSurveyResponseByIDAsync(logicNewSurveyResponse);
                // Try to save the changes and log the outcome.
                try
                {
                    await yuleLogger.logChangedAnswer(checkerClient, logicOldSurveyResponse.surveyQuestion, logicOldSurveyResponse, logicNewSurveyResponse);
                    await repository.SaveAsync();
                    return Ok(await repository.GetSurveyResponseByIDAsync(surveyResponseID));
                }
                // If that fails, revert the answer, post the log, and save it to the DB context
                catch(Exception)
                {
                    await yuleLogger.logError(checkerClient, LoggingConstants.MODIFIED_ANSWER_CATEGORY);
                    return StatusCode(StatusCodes.Status424FailedDependency);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
        }

        // POST: api/SurveyResponse
        [HttpPost]
        [Authorize(Policy = "create:responses")]
        public async Task<ActionResult<Response>> PostSurveyResponse([FromBody, Bind("surveyID, clientID, surveyQuestionID, surveyOptionID, responseText")] Models.Survey_Response_Models.ApiSurveyResponse response)
        {
            try
            {
                Logic.Objects.Response logicResponse = new Logic.Objects.Response()
                {
                    surveyResponseID = Guid.NewGuid(),
                    surveyID = response.surveyID,
                    clientID = response.clientID,
                    surveyQuestion = new Question() { questionID = response.surveyQuestionID },
                    surveyOptionID = response.surveyOptionID,
                    responseText = response.responseText
                };
                await repository.CreateSurveyResponseAsync(logicResponse);
                await repository.SaveAsync();

                return Ok(await repository.GetSurveyResponseByIDAsync(logicResponse.surveyResponseID));
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        // DELETE: api/SurveyResponse/5
        [HttpDelete("{surveyResponseID}")]
        [Authorize(Policy = "delete:responses")]
        public async Task<ActionResult> DeleteSurveyResponse(Guid surveyResponseID)
        {
            Logic.Objects.Response surveyResponse = await repository.GetSurveyResponseByIDAsync(surveyResponseID);
            if (surveyResponse == null)
            {
                return NotFound();
            }
            try
            {
                await repository.DeleteSurveyResponseByIDAsync(surveyResponseID);
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
