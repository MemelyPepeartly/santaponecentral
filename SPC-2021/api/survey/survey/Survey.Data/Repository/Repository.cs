using Microsoft.EntityFrameworkCore;
using Survey.Data.Entities;
using Survey.Logic.Interfaces;
using Survey.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly SantaPoneCentralDatabaseContext santaContext;

        public Repository(SantaPoneCentralDatabaseContext _context)
        {
            santaContext = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        #region Survey
        public async Task CreateSurveyAsync(Logic.Objects.Survey newSurvey)
        {
            Data.Entities.Survey contextSurvey = Mapper.MapSurvey(newSurvey);
            await santaContext.Surveys.AddAsync(contextSurvey);
        }
        public async Task<List<Logic.Objects.Survey>> GetAllSurveys()
        {
            List<Logic.Objects.Survey> surveyList = (await santaContext.Surveys
                .Include(s => s.SurveyQuestionXrefs)
                    .ThenInclude(sqx => sqx.SurveyQuestion.SurveyQuestionOptionXrefs)
                        .ThenInclude(so => so.SurveyOption)
                .Include(s => s.SurveyQuestionXrefs)
                    .ThenInclude(sqx => sqx.SurveyQuestion.SurveyQuestionXrefs)

                .ToListAsync())

                .Select(Mapper.MapSurvey).ToList();

            return surveyList;
        }
        public async Task<Logic.Objects.Survey> GetSurveyByID(Guid surveyId)
        {
            Logic.Objects.Survey logicSurvey = Mapper.MapSurvey(await santaContext.Surveys
                .Include(s => s.SurveyQuestionXrefs)
                    .ThenInclude(sqx => sqx.SurveyQuestion.SurveyQuestionOptionXrefs)
                        .ThenInclude(sqox => sqox.SurveyOption)
                .Include(s => s.SurveyQuestionXrefs)
                    .ThenInclude(sqx => sqx.SurveyQuestion.SurveyQuestionXrefs)

                .Where(s => s.SurveyId == surveyId)
                .AsNoTracking()
                .FirstOrDefaultAsync());
            return logicSurvey;
        }
        public async Task UpdateSurveyByIDAsync(Logic.Objects.Survey targetSurvey)
        {
            Data.Entities.Survey contextOldSurvey = await santaContext.Surveys.FirstOrDefaultAsync(s => s.SurveyId == targetSurvey.surveyID);
            contextOldSurvey.SurveyDescription = targetSurvey.surveyDescription;
            contextOldSurvey.IsActive = targetSurvey.active;
            santaContext.Update(contextOldSurvey);
        }
        public async Task DeleteSurveyByIDAsync(Guid surveyID)
        {
            Data.Entities.Survey contextSurvey = await santaContext.Surveys.FirstOrDefaultAsync(s => s.SurveyId == surveyID);
            santaContext.Surveys.Remove(contextSurvey);
        }
        public async Task DeleteSurveyQuestionXrefBySurveyIDAndQuestionID(Guid surveyID, Guid surveyQuestionID)
        {
            Data.Entities.SurveyQuestionXref contextSurveyQuestionXref = await santaContext.SurveyQuestionXrefs.FirstOrDefaultAsync(sqx => sqx.SurveyId == surveyID && sqx.SurveyQuestionId == surveyQuestionID);
            santaContext.SurveyQuestionXrefs.Remove(contextSurveyQuestionXref);
        }
        #endregion

        #region SurveyOption

        public async Task<List<Option>> GetAllSurveyOption()
        {
            List<Option> listLogicSurveyOption = (await santaContext.SurveyOptions
                .Include(s => s.SurveyQuestionOptionXrefs)
                .Include(s => s.SurveyResponses)
                .ToListAsync())
                .Select(Mapper.MapSurveyOption)
                .ToList();
            return listLogicSurveyOption;
        }

        public async Task<Option> GetSurveyOptionByIDAsync(Guid surveyOptionID)
        {
            Option logicOption = Mapper.MapSurveyOption(await santaContext.SurveyOptions
                .Include(s => s.SurveyResponses)
                .Include(s => s.SurveyQuestionOptionXrefs)
                .FirstOrDefaultAsync(so => so.SurveyOptionId == surveyOptionID));
            return logicOption;
        }

        public async Task CreateSurveyOptionAsync(Option newSurveyOption)
        {
            Data.Entities.SurveyOption contextQuestionOption = Mapper.MapSurveyOption(newSurveyOption);
            await santaContext.SurveyOptions.AddAsync(contextQuestionOption);
        }

        public async Task UpdateSurveyOptionByIDAsync(Option targetSurveyOption)
        {
            Data.Entities.SurveyOption oldOption = await santaContext.SurveyOptions.FirstOrDefaultAsync(o => o.SurveyOptionId == targetSurveyOption.surveyOptionID);

            oldOption.DisplayText = targetSurveyOption.displayText;
            oldOption.SurveyOptionValue = targetSurveyOption.surveyOptionValue;

            santaContext.SurveyOptions.Update(oldOption);
        }

        public async Task DeleteSurveyOptionByIDAsync(Guid surveyOptionID)
        {
            santaContext.SurveyOptions.Remove(await santaContext.SurveyOptions.FirstOrDefaultAsync(o => o.SurveyOptionId == surveyOptionID));
        }
        #endregion

        #region SurveyQuestionOptionXref
        public async Task CreateSurveyQuestionOptionXrefAsync(Option newQuestionOption, Guid surveyQuestionID, bool isActive, int sortOrder)
        {
            try
            {
                Data.Entities.SurveyQuestionOptionXref contextQuestionOptionXref = Mapper.MapQuestionOptionXref(newQuestionOption);
                contextQuestionOptionXref.SurveyQuestionId = surveyQuestionID;
                contextQuestionOptionXref.IsActive = isActive;
                contextQuestionOptionXref.SortOrder = sortOrder;
                await santaContext.SurveyQuestionOptionXrefs.AddAsync(contextQuestionOptionXref);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

        }
        #endregion

        #region Question
        public async Task<List<Question>> GetAllSurveyQuestions()
        {
            List<Question> listLogicQuestion = (await santaContext.SurveyQuestions
                .Include(sq => sq.SurveyResponses)
                .Include(sq => sq.SurveyQuestionOptionXrefs)
                    .ThenInclude(so => so.SurveyOption)
                .Include(sq => sq.SurveyQuestionXrefs)
                .ToListAsync())

                .Select(Mapper.MapQuestion).ToList();
            return listLogicQuestion;
        }
        public async Task<Question> GetSurveyQuestionByIDAsync(Guid questionID)
        {
            Logic.Objects.Question logicQuestion = Mapper.MapQuestion(await santaContext.SurveyQuestions
                .Include(sq => sq.SurveyResponses)
                .Include(sq => sq.SurveyQuestionOptionXrefs)
                    .ThenInclude(sqoxr => sqoxr.SurveyOption)
                .Include(sq => sq.SurveyQuestionXrefs)
                .FirstOrDefaultAsync(q => q.SurveyQuestionId == questionID));
            return logicQuestion;
        }
        public async Task CreateSurveyQuestionAsync(Question newQuestion)
        {
            Entities.SurveyQuestion contextQuestion = Mapper.MapQuestion(newQuestion);
            await santaContext.SurveyQuestions.AddAsync(contextQuestion);
        }
        public async Task CreateSurveyQuestionXrefAsync(Guid surveyID, Guid questionID)
        {
            Data.Entities.SurveyQuestionXref contextQuestionXref = new SurveyQuestionXref()
            {
                SurveyQuestionXrefId = Guid.NewGuid(),
                SurveyQuestionId = questionID,
                SurveyId = surveyID,
                SortOrder = 0,
                IsActive = true
            };
            await santaContext.SurveyQuestionXrefs.AddAsync(contextQuestionXref);
        }
        public async Task UpdateSurveyQuestionByIDAsync(Question targetQuestion)
        {
            Data.Entities.SurveyQuestion oldQuestion = await santaContext.SurveyQuestions.FirstOrDefaultAsync(q => q.SurveyQuestionId == targetQuestion.questionID);

            oldQuestion.QuestionText = targetQuestion.questionText;
            oldQuestion.IsSurveyOptionList = targetQuestion.isSurveyOptionList;
            oldQuestion.SenderCanView = targetQuestion.senderCanView;

            santaContext.SurveyQuestions.Update(oldQuestion);
        }
        public async Task DeleteSurveyQuestionByIDAsync(Guid surveyQuestionID)
        {
            santaContext.SurveyQuestions.Remove(await santaContext.SurveyQuestions.FirstOrDefaultAsync(q => q.SurveyQuestionId == surveyQuestionID));
        }
        #endregion

        #region Response

        public async Task<Logic.Objects.Response> GetSurveyResponseByIDAsync(Guid surveyResponseID)
        {
            Logic.Objects.Response logicResponse = Mapper.MapResponse(await santaContext.SurveyResponses
                .Include(sr => sr.Survey)
                    .ThenInclude(s => s.EventType)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionXrefs)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionOptionXrefs)
                    .ThenInclude(sqox => sqox.SurveyOption)
                .FirstOrDefaultAsync(r => r.SurveyResponseId == surveyResponseID));
            return logicResponse;
        }
        public async Task<List<Logic.Objects.Response>> GetAllSurveyResponsesByClientID(Guid clientID)
        {
            List<Response> listLogicResponse = (await santaContext.SurveyResponses
                .Include(sr => sr.Survey)
                    .ThenInclude(s => s.EventType)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionXrefs)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionOptionXrefs)
                    .ThenInclude(sqox => sqox.SurveyOption)
                .Where(r => r.ClientId == clientID)
                .ToListAsync())
                .Select(Mapper.MapResponse)
                .ToList();
            return listLogicResponse;
        }
        public async Task<List<Logic.Objects.Response>> GetAllSurveyResponses()
        {
            List<Response> listLogicResponse = (await santaContext.SurveyResponses
                .Include(sr => sr.Survey)
                    .ThenInclude(s => s.EventType)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionXrefs)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionOptionXrefs)
                    .ThenInclude(sqox => sqox.SurveyOption)
                .ToListAsync())
                .Select(Mapper.MapResponse)
                .ToList();
            return listLogicResponse;
        }

        public async Task CreateSurveyResponseAsync(Response newResponse)
        {
            Data.Entities.SurveyResponse contextResponse = Mapper.MapResponse(newResponse);
            await santaContext.SurveyResponses.AddAsync(contextResponse);
        }
        public async Task DeleteSurveyResponseByIDAsync(Guid surveyResponseID)
        {
            Data.Entities.SurveyResponse contextResponse = await santaContext.SurveyResponses.FirstOrDefaultAsync(r => r.SurveyResponseId == surveyResponseID);
            santaContext.Remove(contextResponse);
        }
        public async Task UpdateSurveyResponseByIDAsync(Response targetResponse)
        {
            Data.Entities.SurveyResponse contextOldResponse = await santaContext.SurveyResponses.FirstOrDefaultAsync(r => r.SurveyResponseId == targetResponse.surveyResponseID);

            contextOldResponse.ResponseText = targetResponse.responseText;

            santaContext.SurveyResponses.Update(contextOldResponse);
        }
        #endregion

        #region Utility
        public async Task SaveAsync()
        {
            await santaContext.SaveChangesAsync();
        }
        #endregion
    }
}
