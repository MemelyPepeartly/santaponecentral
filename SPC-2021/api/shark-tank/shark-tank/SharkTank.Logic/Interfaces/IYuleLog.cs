using SharkTank.Logic.Objects;
using SharkTank.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTank.Logic.Interfaces
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
        Task logCreatedNewAssignments(BaseClient requestingClient, Client sendingClient, List<string> listNewAssignmentNicknames);
        /// <summary>
        /// Logs a newly created message and the sender reciever information
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="sender"></param>
        /// <param name="reciever"></param>
        /// <returns></returns>
        Task logCreatedNewMessage(BaseClient requestingClient, ClientChatMeta sender, ClientChatMeta reciever);
        /// <summary>
        /// Logs a newly created client
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="newClient"></param>
        /// <returns></returns>
        Task logCreatedNewClient(BaseClient requestingClient, Client newClient);
        /// <summary>
        /// Logs a newly created Auth0 client
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="createdAuth0AccountEmail"></param>
        /// <returns></returns>
        Task logCreatedNewAuth0Client(BaseClient requestingClient, string createdAuth0AccountEmail);
        /// <summary>
        /// Logs a newly created tag
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="newTag"></param>
        /// <returns></returns>
        Task logCreatedNewTag(BaseClient requestingClient, Tag newTag);
        /// <summary>
        /// Logs a newly greated tag relationship on a new client
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="targetClient"></param>
        /// <returns></returns>
        Task logCreatedNewClientTagRelationships(BaseClient requestingClient, BaseClient targetClient);
        #endregion

        #region GET logs
        /// <summary>
        /// Logs when get all clients is called using a requesting client based on the user token
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <returns></returns>
        Task logGetAllClients(BaseClient requestingClient);
        /// <summary>
        /// Logs when a specific client is requested, requesting client as the token client, and the requested client being the client requested
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="requestedClient"></param>
        /// <returns></returns>
        Task logGetSpecificClient(BaseClient requestingClient, Client requestedClient);
        /// <summary>
        /// Logs when a specific client is requested, requesting client as the token client, and the requested client being the client requested. Overload for Base Client objects
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="requestedClient"></param>
        /// <returns></returns>
        Task logGetSpecificClient(BaseClient requestingClient, BaseClient requestedClient);
        /// <summary>
        /// Logs when a profile is gotten. Uses a requester client, and the profile requested
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="returnedProfile"></param>
        /// <returns></returns>
        Task logGetProfile(BaseClient requestingClient, BaseClient requestedClientInfo);
        /// <summary>
        /// Logs a request to gather all histories was made
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <returns></returns>
        Task logGetAllHistories(BaseClient requestingClient);
        /// <summary>
        /// Logs a request for a specific history was made
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="requestedHistory"></param>
        /// <returns></returns>
        Task logGetSpecificHistory(BaseClient requestingClient, BaseClient subjectClient, RelationshipMeta assignmentMeta);

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
        Task logModifiedAnswer(BaseClient requestingClient, Question questionBeingAnsweredFor, string oldAnswer, string newAnswer);
        /// <summary>
        /// Logs when an assignment status is changed with the requestor client, assignment's nickname, the old status, and the new status
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="assignmentNickname"></param>
        /// <param name="oldStatus"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        Task logModifiedAssignmentStatus(BaseClient requestingClient, string assignmentNickname, AssignmentStatus oldStatus, AssignmentStatus newStatus);
        /// <summary>
        /// Overload for normal client object. Logs when an assignment status is changed with the requestor client, assignment's nickname, the old status, and the new status
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
        Task logModifiedProfile(BaseClient requestingClient, Profile modifiedProfile);
        /// <summary>
        /// Logs a client has been changed
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="modifiedClient"></param>
        /// <returns></returns>
        Task logModifiedClient(BaseClient requestingClient, Client modifiedClient);
        /// <summary>
        /// Logs when a message's read status is changed
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="markedMessage"></param>
        /// <returns></returns>
        Task logModifiedMessageReadStatus(BaseClient requestingClient, Message markedMessage);
        /// <summary>
        /// Logs when a client is given a new status
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="affectedClientWithNewStatus"></param>
        /// <param name="oldStatus"></param>
        /// <returns></returns>
        Task logModifiedClientStatus(BaseClient requestingClient, Client affectedClientWithNewStatus, Status oldStatus);

        #endregion

        #region DELETE logs
        /// <summary>
        /// Logs a deleted client object
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="deletedClient"></param>
        /// <returns></returns>
        Task logDeletedClient(BaseClient requestingClient, BaseClient deletedClient);
        /// <summary>
        /// Logs when an assignment is removed from a client
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="affectedClient"></param>
        /// <param name="deletedAssignment"></param>
        /// <returns></returns>
        Task logDeletedAssignment(BaseClient requestingClient, BaseClient affectedClient, RelationshipMeta deletedAssignment);
        /// <summary>
        /// Logs when a tag is deleted
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="deletedTag"></param>
        /// <returns></returns>
        Task logDeletedTag(BaseClient requestingClient, Tag deletedTag);
        #endregion

        #region Utility
        /// <summary>
        /// Error logger which takes in the requestor client, and the category the error occured in
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        Task logError(BaseClient requestingClient, string category);
        /// <summary>
        /// Saves logs with the transient DBContext of the API
        /// </summary>
        /// <returns></returns>
        Task saveLogs();
        #endregion
    }
}
