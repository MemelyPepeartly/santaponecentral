using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Santa.Logic.Objects;
using Santa.Logic.Objects.Base_Objects;
using Santa.Logic.Objects.Base_Objects.Logging;
using Santa.Logic.Objects.Information_Objects;

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
        /// Creates a new client relationship for assignments by a senderID, recipientID, and the eventTypeID, and its assignmentStatusID that the assignment relates to
        /// </summary>
        /// <param name="senderClientID"></param>
        /// <param name="recipientClientID"></param>
        /// <param name="eventTypeID"></param>
        /// <returns></returns>
        Task CreateClientRelationByID(Guid senderClientID, Guid recipientClientID, Guid eventTypeID, Guid assignmentStatusID);
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
        /// Gets a very minimized version of the logic client object by ID. This is for quickly getting info such as the address or email.
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        Task<Client> GetStaticClientObjectByID(Guid clientID);
        /// <summary>
        /// Gets a very minimized version of the logic client object by email. This is for quickly getting info such as the clientID or just address from a client.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Client> GetStaticClientObjectByEmail(string email);
        /// <summary>
        /// Gets a list of all clients
        /// </summary>
        /// <returns></returns>
        Task<List<Logic.Objects.Client>> GetAllClients();
        /// <summary>
        /// Gets a list of minimized client data for parsing and data purposes, less os than convenience and total data
        /// </summary>
        /// <returns></returns>
        Task<List<StrippedClient>> GetAllStrippedClientData();
        /// <summary>
        /// Gets a list of all client ID's quickly
        /// </summary>
        /// <returns></returns>
        Task<List<Guid>> GetAllClientIDs();

        /// <summary>
        /// Updates a client with a logic client object of the target object that reflects what the client should be updated to
        /// </summary>
        /// <param name="targetClient"></param>
        /// <returns></returns>
        Task UpdateClientByIDAsync(Client targetClient);
        /// <summary>
        /// Updates an assignment's progress to a chosen assignment status ID
        /// </summary>
        /// <param name="assignmentID"></param>
        /// <param name="newAssignmentStatusID"></param>
        /// <returns></returns>
        Task UpdateAssignmentProgressStatusByID(Guid assignmentID, Guid newAssignmentStatusID);
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

        #region Assignment Status
        /// <summary>
        /// Creates a new assignment status with the input of a new assignment status logic object
        /// </summary>
        /// <param name="newAssignmentStatus"></param>
        /// <returns></returns>
        Task CreateAssignmentStatus(AssignmentStatus newAssignmentStatus);
        /// <summary>
        /// Gets a logic list of all assignment statuses
        /// </summary>
        /// <returns></returns>
        Task<List<AssignmentStatus>> GetAllAssignmentStatuses();
        /// <summary>
        /// Gets a specific assignment status by its ID
        /// </summary>
        /// <param name="assignmentStatusID"></param>
        /// <returns></returns>
        Task<AssignmentStatus> GetAssignmentStatusByID(Guid assignmentStatusID);
        /// <summary>
        /// Updates a chosen assignment with the target values in a logic assignment status type
        /// </summary>
        /// <param name="targetAssignmentStatus"></param>
        /// <returns></returns>
        Task UpdateAssignmentStatus(AssignmentStatus targetAssignmentStatus);
        /// <summary>
        /// Queues up a delete query for an assignment status by its ID
        /// </summary>
        /// <param name="assignmentStatusID"></param>
        /// <returns></returns>
        Task DeleteAssignmentStatusByID(Guid assignmentStatusID);
        #endregion

        #region Profile
        Task<Profile> GetProfileByEmailAsync(string email);
        Task<Profile> GetProfileByIDAsync(Guid email);
        Task<List<ProfileAssignment>> GetProfileAssignments(Guid clientID);

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
        Task<List<MessageHistory>> GetUnloadedProfileChatHistoriesAsync(Guid profileOwnerClientID);
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

        #region Board Entries
        /// <summary>
        /// Creates a board entry with a new logic board object
        /// </summary>
        /// <param name="newEntry"></param>
        /// <returns></returns>
        Task CreateBoardEntryAsync(BoardEntry newEntry);
        /// <summary>
        /// Gets a logic list of all the board entries
        /// </summary>
        /// <returns></returns>
        Task<List<BoardEntry>> GetAllBoardEntriesAsync();
        /// <summary>
        /// Gets a certain board entry by its boardEntryID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <returns></returns>
        Task<BoardEntry> GetBoardEntryByIDAsync(Guid boardEntryID);
        /// <summary>
        /// Gets a board entry by its post number
        /// </summary>
        /// <param name="postNumber"></param>
        /// <returns></returns>
        Task<BoardEntry> GetBoardEntryByThreadAndPostNumberAsync(int threadNumber, int postNumber);
        /// <summary>
        /// Updates a post number in a board entry using an updated logic board entry object
        /// </summary>
        /// <param name="newEntry"></param>
        /// <returns></returns>
        Task UpdateBoardEntryPostNumberAsync(BoardEntry newEntry);
        /// <summary>
        /// Updates a thread number in a board entry using an updated logic board entry object
        /// </summary>
        /// <param name="newEntry"></param>
        /// <returns></returns>
        Task UpdateBoardEntryThreadNumberAsync(BoardEntry newEntry);
        /// <summary>
        /// Updates a post description in a board entry using an updated logic board entry object
        /// </summary>
        /// <param name="newEntry"></param>
        /// <returns></returns>
        Task UpdateBoardEntryPostDescriptionAsync(BoardEntry newEntry);
        /// <summary>
        /// Updates the entry type of a board post
        /// </summary>
        /// <param name="newEntry"></param>
        /// <returns></returns>
        Task UpdateBoardEntryTypeAsync(BoardEntry newEntry);
        /// <summary>
        /// Deletes a board entry by its board entry ID
        /// </summary>
        /// <param name="boardEntryID"></param>
        /// <returns></returns>
        Task DeleteBoardEntryByIDAsync(Guid boardEntryID);
        #endregion

        #region Entry Type
        Task CreateEntryTypeAsync(EntryType newEntryType);
        Task<List<EntryType>> GetAllEntryTypesAsync();
        Task<EntryType> GetEntryTypeByIDAsync(Guid entryTypeID);
        Task UpdateEntryTypeName(EntryType updatedEntryType);
        Task UpdateEntryTypeDescription(EntryType updatedEntryType);
        Task DeleteEntryTypeByID(Guid entryTypeID);
        #endregion

        #region Note
        Task CreateNoteAsync(Note newNote, Guid clientID);
        Task<List<Note>> GetAllNotesAsync();
        Task<Note> GetNoteByIDAsync(Guid noteID);
        Task UpdateNote(Note updatedNote);
        Task DeleteNoteByID(Guid noteID);
        #endregion

        #region Category
        /// <summary>
        /// Creates a new log category with a filled category object
        /// </summary>
        /// <param name="newCategory"></param>
        /// <returns></returns>
        Task CreateNewCategory(Category newCategory);
        /// <summary>
        /// Gets a list of all the categories
        /// </summary>
        /// <returns></returns>
        Task<List<Category>> GetAllCategories();
        /// <summary>
        /// Gets a specific category by ID
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        Task<Category> GetCategoryByID(Guid categoryID);
        /// <summary>
        /// Updates a category with a filled target category. Searches for the category by ID in the target to update
        /// </summary>
        /// <param name="targetCategory"></param>
        /// <returns></returns>
        Task UpdateCategory(Category targetCategory);
        /// <summary>
        /// Deletes a category by ID
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        Task DeleteCategoryByID(Guid categoryID);
        #endregion

        #region Yule Log
        /// <summary>
        /// Creates a new log entry with a logic YuleLog object
        /// </summary>
        /// <param name="newLog"></param>
        /// <returns></returns>
        Task CreateNewLogEntry(YuleLog newLog);
        /// <summary>
        /// Gets a list of all current log entries
        /// </summary>
        /// <returns></returns>
        Task<List<YuleLog>> GetAllLogEntries();
        /// <summary>
        /// Gets a specific log by ID
        /// </summary>
        /// <param name="logID"></param>
        /// <returns></returns>
        Task<YuleLog> GetLogByID(Guid logID);
        /// <summary>
        /// Gets a list of logs by a category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        Task<List<YuleLog>> GetLogsByCategoryID(Guid categoryID);
        #endregion

        #region Informational
        Task<List<RelationshipMeta>> getClientAssignmentsByIDAsync(Guid clientID);
        Task<List<RelationshipMeta>> getClientSendersByIDAsync(Guid clientID);
        #endregion

        #region Utility
        /// <summary>
        /// Saves changes of any CRUD operations in the queue
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();

        /// <summary>
        /// Gets a list of all the allowed assignments for a client by their ID, and the event's ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="eventTypeID"></param>
        /// <returns></returns>
        Task<List<AllowedAssignmentMeta>> GetAllAllowedAssignmentsByID(Guid clientID, Guid eventTypeID);
        /// <summary>
        /// Search a list of clients and returns a list based on a model of queries
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        Task<List<StrippedClient>> SearchClientByQuery(SearchQueries searchQuery);
        #endregion

    }
}