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
        List<Logic.Objects.Client> GetAllClients();
        Task<Logic.Objects.Client> UpdateClientByIDAsync();
        Task<Logic.Objects.Client> DeleteClientByIDAsync();
        #endregion

        #region Event
        Task CreateEventAsync(Event newEvent);
        List<Logic.Objects.Event> GetAllEvents();
        Task<Logic.Objects.Event> GetEventByIDAsync(Guid eventID);
        Task<Logic.Objects.Event> UpdateEventByIDAsync();
       
        Task<Logic.Objects.Event> DeleteEventByIDAsync();
        #endregion

        #region Status
        Task CreateStatusAsync(Status newStatus);
        Task<Status> GetClientStatusByID(Guid clientStatusID);
        List<Status> GetAllClientStatus();
        Task UpdateStatusByIDAsync(Guid clientStatusID, Status changedLogicStatus);
        Task DeleteStatusByIDAsync(Guid clientStatusID);
        #endregion

        #region Surveys

        Task<Logic.Objects.Survey> GetSurveyByID(Guid id);
        Task CreateSurveyAsync(Survey newSurvey);

        #region SurveyOptions
        Task CreateSurveyOptionAsync(Option newQuestionOption);
        Task CreateSurveyQuestionOptionXrefAsync(Option newQuestionOption);
        Task<Logic.Objects.Question> UpdateSurveyOptionByIDAsync();
        Task<Logic.Objects.Question> DeleteSurveyOptionByIDAsync();
        #endregion

        #region SurveyQuestions
        Task CreateSurveyQuestionXrefAsync(Logic.Objects.Question logicQuestion);
        Task CreateSurveyQuestionAsync(Question newQuestion);
        List<Logic.Objects.Survey> GetAllSurveys();
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