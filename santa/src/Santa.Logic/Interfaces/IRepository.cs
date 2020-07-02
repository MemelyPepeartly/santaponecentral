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
        Task<Logic.Objects.Client> GetClientByIDAsync(Guid clientId);
        Task<List<Logic.Objects.Client>> GetAllClients();
        Task UpdateClientByIDAsync(Client targetClient);
        Task DeleteClientByIDAsync(Guid eventID);
        Task DeleteRecieverXref(Guid clientID, Guid recipientID, Guid eventID);
        #endregion

        #region Profile
        Task<Profile> GetProfileByEmailAsync(string email);

        #endregion

        #region Tag
        Task CreateTag(Tag newTag);
        Task CreateClientTagRelationByID(Guid clientID, Guid tagID);
        Task<Tag> GetTagByIDAsync(Guid tagID);
        Task<List<Tag>> GetAllTags();
        Task UpdateTagNameByIDAsync(Tag logicTag);
        Task DeleteClientTagRelationshipByID(Guid clientID, Guid tagID);
        Task DeleteTagByIDAsync(Guid tagID);
        #endregion

        #region Event
        Task CreateEventAsync(Event newEvent);
        Task<List<Logic.Objects.Event>> GetAllEvents();
        Task<Logic.Objects.Event> GetEventByIDAsync(Guid eventID);
        Task UpdateEventByIDAsync(Event targetEvent);
        Task DeleteEventByIDAsync(Guid logicEvent);
        #endregion

        #region Message
        Task CreateMessage(Message newMessage);
        Task<List<Message>> GetAllMessages();
        Task<Logic.Objects.Message> GetMessageByIDAsync(Guid chatMessageID);
        Task UpdateMessageByIDAsync(Message targetMessage);

        #region Message Histories
        // General getters
        Task<List<MessageHistory>> GetAllChatHistories();
        // Client getters
        Task<List<MessageHistory>> GetAllChatHistoriesByClientIDAsync(Guid clientID);
        Task<MessageHistory> GetChatHistoryByClientIDAndRelationXrefIDAsync(Guid clientID, Guid clientRelationXrefID);
        Task<MessageHistory> GetGeneralChatHistoryByClientIDAsync(Guid clientID);
        // Event getters
        Task<List<MessageHistory>> GetAllChatHistoriesByEventIDAsync(Guid eventID);


        #endregion

        #endregion

        #region Status
        Task CreateStatusAsync(Status newStatus);
        Task<Status> GetClientStatusByID(Guid clientStatusID);
        Task<List<Status>> GetAllClientStatus();
        Task UpdateStatusByIDAsync(Status targetStatus);
        Task DeleteStatusByIDAsync(Guid clientStatusID);
        #endregion

        #region Surveys
        Task CreateSurveyAsync(Survey newSurvey);
        Task<List<Survey>> GetAllSurveys();
        Task<Survey> GetSurveyByID(Guid id);
        Task UpdateSurveyByIDAsync(Survey targetSurvey);
        Task DeleteSurveyByIDAsync(Guid surveyID);
        Task DeleteSurveyQuestionXrefBySurveyIDAndQuestionID(Guid surveyID, Guid surveyQuestionID);


        #region SurveyResponses
        Task<List<Logic.Objects.Response>> GetAllSurveyResponses();
        Task<List<Logic.Objects.Response>> GetAllSurveyResponsesByClientID(Guid clientID);
        Task CreateSurveyResponseAsync(Response newResponse);
        Task<Logic.Objects.Response> GetSurveyResponseByIDAsync(Guid surveyResponseID);
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
        Task CreateSurveyQuestionOptionXrefAsync(Option newQuestionOption, Guid surveyQuestionID, bool isActive, string sortOrder);
        #endregion

        #region SurveyQuestions
        Task<List<Question>> GetAllSurveyQuestions();
        Task<Question> GetSurveyQuestionByIDAsync(Guid questionID);
        Task CreateSurveyQuestionXrefAsync(Logic.Objects.Question logicQuestion);
        Task CreateSurveyQuestionAsync(Question newQuestion);

        Task UpdateSurveyQuestionByIDAsync(Question targetQuestion);
        Task DeleteSurveyQuestionByIDAsync(Guid surveyQuestionID);
        #endregion

        #region Utility
        Task SaveAsync();
        #endregion

    }
}