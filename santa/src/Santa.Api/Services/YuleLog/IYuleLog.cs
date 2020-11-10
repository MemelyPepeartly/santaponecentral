using Santa.Logic.Objects;
using Santa.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Services.YuleLog
{
    public interface IYuleLog
    {
        #region POST logs
        /// <summary>
        /// Logs new created assignments with a list of the nicknames of new assignments for a sending client
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="sendingClient"></param>
        /// <param name="listNewAssignmentNicknames"></param>
        /// <returns></returns>
        Task logCreatedNewAssignments(Client requestingClient, Client sendingClient, List<string> listNewAssignmentNicknames);
        #endregion

        #region GET logs
        /// <summary>
        /// Logs when get all clients is called using a requesting client based on the user token
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <returns></returns>
        Task logGetAllClients(Client requestingClient);
        /// <summary>
        /// Logs when a specific client is requested, requesting client as the token client, and the requested client being the client requested
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="requestedClient"></param>
        /// <returns></returns>
        Task logGetSpecificClient(Client requestingClient, Client requestedClient);
        /// <summary>
        /// Logs when a profile is gotten. Uses a requester client, and the profile requested
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="returnedProfile"></param>
        /// <returns></returns>
        Task logGetProfile(Client requestingClient, Profile returnedProfile);
        /// <summary>
        /// Logs a request to gather all histories was made
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <returns></returns>
        Task logGetAllHistories(Client requestingClient);
        /// <summary>
        /// Logs a request for a specific history was made
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="requestedHistory"></param>
        /// <returns></returns>
        Task logGetSpecificHistory(Client requestingClient, MessageHistory requestedHistory);
        
        #endregion

        #region PUT logs
        /// <summary>
        /// Logs when an answer has been changed using the requestor client, question, and the change in answers
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="questionBeingAnsweredFor"></param>
        /// <param name="oldAnswer"></param>
        /// <param name="newAnswer"></param>
        /// <returns></returns>
        Task logModifiedAnswer(Client requestingClient, Question questionBeingAnsweredFor, string oldAnswer, string newAnswer);
        /// <summary>
        /// Logs when an assignment status is changed with the requestor client, assignment's nickname, the old status, and the new status
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="assignmentNickname"></param>
        /// <param name="oldStatus"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        Task logModifiedAssignmentStatus(Client requestingClient, string assignmentNickname, AssignmentStatus oldStatus, AssignmentStatus newStatus);
        /// <summary>
        /// Logs a change to a profile has been made
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="modifiedProfile"></param>
        /// <returns></returns>
        Task logModifiedProfile(Client requestingClient, Profile modifiedProfile);
        /// <summary>
        /// Logs a client has been changed
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="modifiedClient"></param>
        /// <returns></returns>
        Task logModifiedClient(Client requestingClient, Client modifiedClient);

        #endregion

        #region DELETE logs
        #endregion

        #region Utility
        /// <summary>
        /// Error logger which takes in the requestor client, and the category the error occured in
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        Task logError(Client requestingClient, string category);
        /// <summary>
        /// Saves logs with the transient DBContext of the API
        /// </summary>
        /// <returns></returns>
        Task saveLogs();
        #endregion

        Task logCreatedNewMessage(Client requestingClient, ClientChatMeta sender, ClientChatMeta reciever);
        Task logCreatedNewClient(Client requestingClient);
        Task logCreatedNewAuth0Client(Client requestingClient);
        Task logCreatedNewTag(Client requestingClient, Tag newTag);
        Task logCreatedNewClientTagRelationship(Client requestingClient);
        Task logDeletedClient(Client requestingClient);
        Task logDeletedAssignment(Client requestingClient);
        Task logDeletedTag(Client requestingClient);
        Task logModifiedMessageReadStatus(Client requestingClient);
        Task logModifiedClientStatus(Client requestingClient);

    }
}
