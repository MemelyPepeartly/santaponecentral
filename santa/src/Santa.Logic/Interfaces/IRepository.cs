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
        /// <summary>
        /// Creates a new client by a logic client object
        /// </summary>
        /// <param name="newClient"></param>
        /// <returns></returns>
        Task CreateClient(Client newClient);
        /// <summary>
        /// Creates a new client relationship for assignments by a senderID, recipientID, and the eventTypeID that the assignment relates to
        /// </summary>
        /// <param name="senderClientID"></param>
        /// <param name="recipientClientID"></param>
        /// <param name="eventTypeID"></param>
        /// <returns></returns>
        Task CreateClientRelationByID(Guid senderClientID, Guid recipientClientID, Guid eventTypeID);
        /// <summary>
        /// Gets a client by their ID
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<Logic.Objects.Client> GetClientByIDAsync(Guid clientId);
        /// <summary>
        /// Gets a client by their email
        /// </summary>
        /// <param name="clientEmail"></param>
        /// <returns></returns>
        Task<Logic.Objects.Client> GetClientByEmailAsync(string clientEmail);
        /// <summary>
        /// Gets a list of all clients
        /// </summary>
        /// <returns></returns>
        Task<List<Logic.Objects.Client>> GetAllClients();
        /// <summary>
        /// Updates a client with a logic client object of the target object that reflects what the client should be updated to
        /// </summary>
        /// <param name="targetClient"></param>
        /// <returns></returns>
        Task UpdateClientByIDAsync(Client targetClient);
        /// <summary>
        /// Updates the completion status of a relation (Assignmnent) by a senderID, recipientID, eventTypeID, and the targeted completion status
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="recipientID"></param>
        /// <param name="eventTypeID"></param>
        /// <param name="targetCompletedStatus"></param>
        /// <returns></returns>
        Task UpdateClientRelationCompletedStatusByID(Guid senderID, Guid recipientID, Guid eventTypeID, bool targetCompletedStatus);
        /// <summary>
        /// Deletes a client by their ID along with any data about them. This includes chat histories, relationships, and answers
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        Task DeleteClientByIDAsync(Guid clientID);
        /// <summary>
        /// Deletes a reciever from the client by the client's ID, and the IDs of the recipient and event in question
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="recipientID"></param>
        /// <param name="eventID"></param>
        /// <returns></returns>
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
        Task<Event> GetEventByNameAsync(string eventName);
        Task UpdateEventByIDAsync(Event targetEvent);
        Task DeleteEventByIDAsync(Guid logicEvent);
        #endregion

        #region Message
        Task CreateMessage(Message newMessage);
        Task<List<Message>> GetAllMessages();
        Task<Logic.Objects.Message> GetMessageByIDAsync(Guid chatMessageID);
        Task UpdateMessageByIDAsync(Message targetMessage);

        #region Message Histories
        /* Subject ID's are needed to determine who was the client that made the call. For example, if a person with a profile wants their messages,
           they will call the endpoint with themselves as the subject
        */
        Task<List<MessageHistory>> GetAllChatHistories(Client subjectClient);
        Task<List<MessageHistory>> GetAllChatHistoriesBySubjectIDAsync(Client subjectClient);
        Task<MessageHistory> GetChatHistoryByXrefIDAndSubjectIDAsync(Guid clientRelationXrefID, Client subjectClient);
        Task<MessageHistory> GetGeneralChatHistoryBySubjectIDAsync(Client conversationClient, Client subjectClient);
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
        /// <summary>
        /// Creates a relationship between a survey and a question based on their ID
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="questionID"></param>
        /// <returns></returns>
        Task CreateSurveyQuestionXrefAsync(Guid surveyID, Guid questionID);
        Task CreateSurveyQuestionAsync(Question newQuestion);

        Task UpdateSurveyQuestionByIDAsync(Question targetQuestion);
        Task DeleteSurveyQuestionByIDAsync(Guid surveyQuestionID);
        #endregion

        #region Board Entries
        Task CreateBoardEntryAsync(BoardEntry newEntry);
        Task<List<BoardEntry>> GetAllBoardEntriesAsync();
        Task<BoardEntry> GetBoardEntryByIDAsync(Guid boardEntryID);
        Task<BoardEntry> GetBoardEntryByPostNumberAsync(int postNumber);
        Task UpdateBoardEntryAsync(BoardEntry newEntry);
        Task DeleteBoardEntryAsync();
        #endregion

        #region Utility
        Task SaveAsync();
        #endregion

    }
}