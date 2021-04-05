using Survey.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Logic.Interfaces
{
    public interface IRepository
    {
        #region Surveys
        Task CreateSurveyAsync(Objects.Survey newSurvey);
        Task<List<Objects.Survey>> GetAllSurveys();
        Task<Objects.Survey> GetSurveyByID(Guid id);
        Task UpdateSurveyByIDAsync(Objects.Survey targetSurvey);
        Task DeleteSurveyByIDAsync(Guid surveyID);
        Task DeleteSurveyQuestionXrefBySurveyIDAndQuestionID(Guid surveyID, Guid surveyQuestionID);


        #region SurveyResponses
        Task<List<Response>> GetAllSurveyResponses();
        Task<List<Response>> GetAllSurveyResponsesByClientID(Guid clientID);
        Task CreateSurveyResponseAsync(Response newResponse);
        Task<Response> GetSurveyResponseByIDAsync(Guid surveyResponseID);
        Task UpdateSurveyResponseByIDAsync(Response targetResponse);
        Task DeleteSurveyResponseByIDAsync(Guid surveyResponseID);

        #endregion

        #endregion

        #region SurveyOption
        Task<List<Option>> GetAllSurveyOption();
        Task<Option> GetSurveyOptionByIDAsync(Guid surveyOptionID);
        Task UpdateSurveyOptionByIDAsync(Option targetSurveyOption);
        Task DeleteSurveyOptionByIDAsync(Guid surveyOptionID);
        #endregion

        #region SurveyQuestionOptionXref
        Task CreateSurveyOptionAsync(Option newQuestionOption);
        Task CreateSurveyQuestionOptionXrefAsync(Option newQuestionOption, Guid surveyQuestionID, bool isActive, int sortOrder);
        #endregion

        #region SurveyQuestions
        Task CreateSurveyQuestionAsync(Question newQuestion);
        /// <summary>
        /// Creates a relationship between a survey and a question based on their ID
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="questionID"></param>
        /// <returns></returns>
        Task CreateSurveyQuestionXrefAsync(Guid surveyID, Guid questionID);
        Task<List<Question>> GetAllSurveyQuestions();
        Task<Question> GetSurveyQuestionByIDAsync(Guid questionID);
        Task UpdateSurveyQuestionByIDAsync(Question targetQuestion);
        Task DeleteSurveyQuestionByIDAsync(Guid surveyQuestionID);
        #endregion

        #region Utility
        /// <summary>
        /// Saves changes of any CRUD operations in the queue
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
        #endregion
    }
}
