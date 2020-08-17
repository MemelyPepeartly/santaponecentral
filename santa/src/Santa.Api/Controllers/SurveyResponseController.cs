﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;


namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SurveyResponseController : ControllerBase
    {
        private readonly IRepository repository;
        public SurveyResponseController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
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
#warning clients and admins can do this. Needs check to ensure the response is coming from the right person
            try
            {
                Logic.Objects.Response logicSurveyResponse = await repository.GetSurveyResponseByIDAsync(surveyResponseID);
                logicSurveyResponse.responseText = responseText.responseText;

                await repository.UpdateSurveyResponseByIDAsync(logicSurveyResponse);
                await repository.SaveAsync();

                return Ok(await repository.GetSurveyResponseByIDAsync(surveyResponseID));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // POST: api/SurveyResponse
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
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
