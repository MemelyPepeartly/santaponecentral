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
        Task CreateClientRelationByID(Guid senderClientID, Guid recipientClientID, Guid eventTypeID);
        Task<Logic.Objects.Client> GetClientByID(Guid clientId);
        List<Logic.Objects.Client> GetAllClients();
        Task UpdateClientByIDAsync(Client targetClient);
        Task<Logic.Objects.Client> DeleteClientByIDAsync(Guid eventID);
        #endregion

        #region Event
        Task CreateEventAsync(Event newEvent);
        List<Logic.Objects.Event> GetAllEvents();
        Task<Logic.Objects.Event> GetEventByIDAsync(Guid eventID);
        Task UpdateEventByIDAsync(Event targetEvent);
       
        Task DeleteEventByIDAsync(Guid logicEvent);
        #endregion

        #region Status
        Task CreateStatusAsync(Status newStatus);
        Task<Status> GetClientStatusByID(Guid clientStatusID);
        List<Status> GetAllClientStatus();
        Task UpdateStatusByIDAsync(Status targetStatus);
        Task DeleteStatusByIDAsync(Guid clientStatusID);
        #endregion

        #region Surveys
        Task CreateSurveyAsync(Survey newSurvey);
        List<Survey> GetAllSurveys();
        Task<Survey> GetSurveyByID(Guid id);
        Task UpdateSurveyByIDAsync(Survey targetSurvey);
        Task DeleteSurveyByIDAsync(Guid surveyID);
        Task DeleteSurveyQuestionXrefBySurveyIDAndQuestionID(Guid surveyID, Guid surveyQuestionID);

        #region SurveyOptions
        Task CreateSurveyOptionAsync(Option newQuestionOption);
        Task CreateSurveyQuestionOptionXrefAsync(Option newQuestionOption);
        Task<Logic.Objects.Question> UpdateSurveyOptionByIDAsync();
        Task<Logic.Objects.Question> DeleteSurveyOptionByIDAsync();
        #endregion

        #region SurveyQuestions
        List<Question> GetAllSurveyQuestions();
        Task<Question> GetSurveyQuestionByIDAsync(Guid questionID);
        Task CreateSurveyQuestionXrefAsync(Logic.Objects.Question logicQuestion);
        Task CreateSurveyQuestionAsync(Question newQuestion);
        
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