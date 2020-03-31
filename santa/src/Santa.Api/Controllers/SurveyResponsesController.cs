using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Santa.Data.Entities;
using Santa.Logic.Interfaces;

namespace Santa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyResponsesController : ControllerBase
    {
        private readonly IRepository repository;
        public SurveyResponsesController(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        // GET: api/SurveyResponses
        [HttpGet]
        public async Task<ActionResult<List<Logic.Objects.Response>>> GetSurveyResponse()
        {
            return Ok(await repository.GetAllSurveyResponses());
        }

        // GET: api/SurveyResponses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyResponse>> GetSurveyResponse(Guid surveyResponseID)
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
        [HttpPut("{id}/ResponseText")]
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
        public async Task<ActionResult<SurveyResponse>> PostSurveyResponse([FromBody, Bind("surveyID, clientID, surveyQuestionID, surveyOptionID, responseText")] Models.Survey_Response_Models.ApiSurveyResponse response)
        {
            try
            {
                Logic.Objects.Response logicResponse = new Logic.Objects.Response()
                {
                    surveyResponseID = Guid.NewGuid(),
                    surveyID = response.surveyID,
                    clientID = response.clientID,
                    surveyQuestionID = response.surveyQuestionID,
                    surveyOptionID = response.surveyOptionID,
                    responseText = response.responseText
                };
                await repository.CreateSurveyResponseAsync(logicResponse);
                await repository.SaveAsync();

                return Ok(await repository.GetSurveyResponseByIDAsync(logicResponse.surveyResponseID));
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        // DELETE: api/SurveyResponses/5
        [HttpDelete("{id}")]
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
