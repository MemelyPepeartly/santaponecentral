using Santa.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Services.YuleLog
{
    public interface IYuleLog
    {
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
        /// Logs when an answer has been changed using the requestor client, question, and the change in answers
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="questionBeingAnsweredFor"></param>
        /// <param name="oldAnswer"></param>
        /// <param name="newAnswer"></param>
        /// <returns></returns>
        Task logChangedAnswer(Client requestingClient, Question questionBeingAnsweredFor, string oldAnswer, string newAnswer);
        /// <summary>
        /// Logs when an assignment status is changed with the requestor client, assignment's nickname, the old status, and the new status
        /// </summary>
        /// <param name="requestingClient"></param>
        /// <param name="assignmentNickname"></param>
        /// <param name="oldStatus"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        Task logChangedAssignmentStatus(Client requestingClient, string assignmentNickname, AssignmentStatus oldStatus, AssignmentStatus newStatus);
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
    }
}
