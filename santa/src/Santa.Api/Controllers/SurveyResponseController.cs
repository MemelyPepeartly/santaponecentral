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


namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyResponseController : ControllerBase
    {
        private readonly IRepository repository;
        public SurveyResponseController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }
        
        // GET: api/SurveyResponses
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "read:responses")]

        public async Task<ActionResult<List<Logic.Objects.Response>>> GetSurveyResponse()
        {
            return Ok(await repository.GetAllSurveyResponses());
        }
        
        // GET: api/SurveyResponses/5
        [HttpGet("{surveyResponseID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "read:responses")]

        public async Task<ActionResult<Response>> GetSurveyResponse(Guid surveyResponseID)
        {
            Logic.Objects.Response surveyResponse = await repository.GetSurveyResponseByIDAsync(surveyResponseID);

            if (surveyResponse == null)
            {
                return NotFound();
            }

            return Ok(surveyResponse);
        }

        // PUT: api/SurveyResponses/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{surveyResponseID}/ResponseText")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "modify:responses")]

        public async Task<ActionResult<Logic.Objects.Response>> PutSurveyResponse(Guid surveyResponseID, Models.Survey_Response_Models.ApiSurveyReponseText responseText)
        {
            
            try
            {
                Logic.Objects.Response surveyResponse = await repository.GetSurveyResponseByIDAsync(surveyResponseID);
                surveyResponse.responseText = responseText.responseText;

                await repository.UpdateSurveyResponseByIDAsync(surveyResponse);
                await repository.SaveAsync();

                return Ok(await repository.GetSurveyResponseByIDAsync(surveyResponseID));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        // POST: api/SurveyResponses
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
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

        // DELETE: api/SurveyResponses/5
        [HttpDelete("{surveyResponseID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "delete:responses")]

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
