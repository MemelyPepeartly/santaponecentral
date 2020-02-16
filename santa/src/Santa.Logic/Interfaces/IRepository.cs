using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Santa.Logic.Objects;

namespace Santa.Logic.Interfaces
{
    public interface IRepository
    {
        #region Client
        Task CreateClient(Client newClient);
        Task<Logic.Objects.Client> GetClientByID(Guid clientId);
        Task<Logic.Objects.Client> GetClientByEmailAsync();
        List<Logic.Objects.Client> GetAllClients();
        Task<Logic.Objects.Client> UpdateClientByIDAsync();
        Task<Logic.Objects.Client> DeleteClientByIDAsync();
        #endregion

        #region Event
        Task<Logic.Objects.Event> CreateEventAsync();
        List<Logic.Objects.Event> GetAllEvents();
        Task<Logic.Objects.Event> GetEventByIDAsync();
        Task<Logic.Objects.Event> UpdateEventByIDAsync();
        Task<Logic.Objects.Event> DeleteEventByIDAsync();
        #endregion

        #region Surveys

        Task<Logic.Objects.Survey> GetSurveyByID(Guid id);
        void CreateSurvey(Survey newSurvey);

        #region SurveyOptions
        Task<Logic.Objects.Question> CreateSurveyOptionAsync();
        Task<Logic.Objects.Question> GetSurveyOptionByIDAsync();
        Task<Logic.Objects.Question> UpdateSurveyOptionByIDAsync();
        Task<Logic.Objects.Question> DeleteSurveyOptionByIDAsync();
        #endregion

        #region SurveyQuestions
        Task CreateSurveyQuestionXref(Guid surveyId, Logic.Objects.Question contextQuestion);
        Task CreateSurveyQuestionAsync(Question newQuestion);
        List<Logic.Objects.Survey> GetAllSurveys();
        Task<List<Question>> GetSurveyQuestionsBySurveyIDAsync(Guid id);
        Task<Logic.Objects.Question> UpdateSurveyQuestionByIDAsync();
        Task<Logic.Objects.Question> DeleteSurveyQuestionByIDAsync();
        #endregion

        #region SurveyResponses
        Task<Logic.Objects.Response> CreateSurveyResponseAsync();
        Task<Logic.Objects.Response> GetSurveyResponseByIDAsync();
        Task<Logic.Objects.Response> UpdateSurveyResponseByIDAsync();
        Task<Logic.Objects.Response> DeleteSurveyResponseByIDAsync();
        #endregion
        #endregion

        public Task SaveAsync();
        
    }
}