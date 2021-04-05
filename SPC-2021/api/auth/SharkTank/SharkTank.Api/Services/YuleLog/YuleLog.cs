using SharkTank.Logic.Constants;
using SharkTank.Logic.Interfaces;
using SharkTank.Logic.Objects;
using SharkTank.Logic.Objects.Base_Objects.Logging;
using SharkTank.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SharkTank.Api.Services.YuleLog
{
    public class YuleLog : IYuleLog
    {
        private readonly IRepository repository;

        public YuleLog(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        #region POST logs
        public async Task logCreatedNewAssignments(BaseClient requestingClient, Client sendingClient, List<string> listNewAssignmentNicknames)
        {
            string logMessage = $"{requestingClient.nickname} made a request to add the following assignments to {sendingClient.nickname}: ";
            foreach(string assignmentNickname in listNewAssignmentNicknames.Take(4))
            {
                logMessage += assignmentNickname;
                if (!listNewAssignmentNicknames.Take(4).ToList().IndexOf(assignmentNickname).Equals(listNewAssignmentNicknames.Take(4).ToList().Count -1))
                {
                    logMessage += ", ";
                }
                else if(listNewAssignmentNicknames.Count > 4)
                {
                    logMessage += ", and more";
                }
            }

            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.CREATED_ASSIGNMENT_CATEGORY), logMessage));
            await saveLogs();
        }
        public async Task logCreatedNewMessage(BaseClient requestingClient, ClientChatMeta sender, ClientChatMeta receiver)
        {
            string receiverNickname = receiver.clientNickname;
            if (receiver.clientId == null)
            {
                receiverNickname = "Event Organizers";
            }
            string logMessage = $"{requestingClient.nickname} requested to post a new message. Sender: {sender.clientNickname} | Reciever: {receiverNickname} ";
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.CREATED_NEW_MESSAGE_CATEGORY), logMessage));
            await saveLogs();
        }
        public async Task logCreatedNewClient(BaseClient requestingClient, Client newClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.CREATED_NEW_CLIENT_CATEGORY), $"{requestingClient.nickname} requested to make a new client object with email: {newClient.email}"));
            await saveLogs();
        }

        public async Task logCreatedNewAuth0Client(BaseClient requestingClient, string createdAuth0AccountEmail)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.CREATED_NEW_AUTH0_CLIENT_CATEGORY), $"{requestingClient.nickname} requested to make a client an Auth0 account for the email: {createdAuth0AccountEmail}"));
            await saveLogs();
        }

        public async Task logCreatedNewTag(BaseClient requestingClient, Logic.Objects.Tag newTag)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.CREATED_NEW_TAG_CATEGORY), $"{requestingClient.nickname} requested to make a new tag: {newTag.tagName}"));
            await saveLogs();
        }

        public async Task logCreatedNewClientTagRelationships(BaseClient requestingClient, BaseClient targetClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.CREATED_NEW_CLIENT_TAG_RELATIONSHIPS_CATEGORY), $"{requestingClient.nickname} requested to add tags to {targetClient.nickname}"));
            await saveLogs();
        }
        #endregion

        #region GET logs
        public async Task logGetAllClients(BaseClient requestingClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_ALL_CLIENT_CATEGORY), requestingClient.nickname + " made a request to retrieve a list of all clients"));
            await saveLogs();
        }
        public async Task logGetSpecificClient(BaseClient requestingClient, Client requestedClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_SPECIFIC_CLIENT_CATEGORY), requestingClient.nickname + " made a request to retrieve the client information for " + requestedClient.nickname));
            await saveLogs();
        }
        public async Task logGetSpecificClient(BaseClient requestingClient, BaseClient requestedClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_SPECIFIC_CLIENT_CATEGORY), requestingClient.nickname + " made a request to retrieve the client information for " + requestedClient.nickname));
            await saveLogs();
        }
        public async Task logGetProfile(BaseClient requestingClient, Profile returnedProfile)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_PROFILE_CATEGORY), requestingClient.nickname + " made a request to get a profile object for: " + returnedProfile.nickname));
            await saveLogs();
        }
        public async Task logGetAllHistories(BaseClient requestingClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_ALL_HISTORY_CATEGORY), requestingClient.nickname + " made a request to retrieve a list of all chat histories"));
            await saveLogs();
        }
        public async Task logGetSpecificHistory(BaseClient requestingClient, MessageHistory requestedHistory)
        {
            string logMessage = requestedHistory.relationXrefID != null ? $"{requestingClient.nickname} requested to get a history between admins and {requestedHistory.conversationClient.clientNickname} about an assignment ({requestedHistory.assignmentRecieverClient.clientNickname}) " : $"{requestingClient.nickname} requested to get a general history between admins and {requestedHistory.conversationClient.clientNickname}";
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_SPECIFIC_HISTORY_CATEGORY), logMessage));
            await saveLogs();
        }
        #endregion

        #region PUT logs
        public async Task logModifiedAnswer(BaseClient requestingClient, Question questionBeingAnsweredFor, string oldAnswer, string newAnswer)
        {
            string logMessage = $@"{requestingClient.nickname} made a request to change their answer for question '{questionBeingAnsweredFor.questionText}'";
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.MODIFIED_ANSWER_CATEGORY), logMessage));
            await saveLogs();
        }
        public async Task logModifiedAssignmentStatus(BaseClient requestingClient, string assignmentNickname, AssignmentStatus oldStatus, AssignmentStatus newStatus)
        {
            string logMessage = $@"{requestingClient.nickname} made a request to change the assignment status for {assignmentNickname} from '{oldStatus.assignmentStatusName}' to '{newStatus.assignmentStatusName}'";
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.MODIFIED_ASSIGNMENT_STATUS_CATEGORY), logMessage));
            await saveLogs();
        }
        public async Task logModifiedAssignmentStatus(Client requestingClient, string assignmentNickname, AssignmentStatus oldStatus, AssignmentStatus newStatus)
        {
            string logMessage = $@"{requestingClient.nickname} made a request to change the assignment status for {assignmentNickname} from '{oldStatus.assignmentStatusName}' to '{newStatus.assignmentStatusName}'";
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.MODIFIED_ASSIGNMENT_STATUS_CATEGORY), logMessage));
            await saveLogs();
        }
        public async Task logModifiedProfile(BaseClient requestingClient, Profile modifiedProfile)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_ALL_HISTORY_CATEGORY), $"{requestingClient.nickname} modified data on a profile for {modifiedProfile.nickname}"));
            await saveLogs();
        }
        public async Task logModifiedClient(BaseClient requestingClient, Client modifiedClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_ALL_HISTORY_CATEGORY), $"{requestingClient.nickname} modified a client's data for {modifiedClient.nickname}"));
            await saveLogs();
        }
        public async Task logModifiedMessageReadStatus(BaseClient requestingClient, Message markedMessage)
        {
            string receiverNickname = markedMessage.recieverClient.clientNickname;
            if (markedMessage.recieverClient.clientId == null)
            {
                receiverNickname = "the Event Organizers";
            }
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.MODIFIED_MESSAGE_READ_STATUS_CATEGORY), $"{requestingClient.nickname} requested to mark a past message as read where the sender Client was {markedMessage.senderClient.clientNickname} and reciever was {receiverNickname}"));
            await saveLogs();
        }

        public async Task logModifiedClientStatus(BaseClient requestingClient, Client affectedClientWithNewStatus, Status oldStatus)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.MODIFIED_CLIENT_STATUS_CATEGORY), $"{requestingClient.nickname} requested to modify {affectedClientWithNewStatus.nickname}'s status from '{oldStatus.statusDescription}' to '{affectedClientWithNewStatus.clientStatus.statusDescription}'"));
            await saveLogs();
        }
        #endregion

        #region DELETE logs
        public async Task logDeletedClient(BaseClient requestingClient, BaseClient deletedClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.DELETED_CLIENT_CATEGORY), $"{requestingClient.nickname} requested to delete {deletedClient.nickname} and all related entities"));
            await saveLogs();
        }

        public async Task logDeletedAssignment(BaseClient requestingClient, BaseClient affectedClient, RelationshipMeta deletedAssignment)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.DELETED_ASSIGNMENT_CATEGORY), $"{requestingClient.nickname} requested to remove {deletedAssignment.relationshipClient.clientNickname} from {affectedClient.nickname}'s assignment list"));
            await saveLogs();

        }

        public async Task logDeletedTag(BaseClient requestingClient, Logic.Objects.Tag deletedTag)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.DELETED_TAG_CATEGORY), $"{requestingClient.nickname} requested to delete the {deletedTag.tagName} tag"));
            await saveLogs();
        }
        #endregion

        #region Utility
        public async Task logError(BaseClient requestingClient, string category)
        {
            Category errorCategory = await getCategoryByName(LoggingConstants.MODIFIED_ANSWER_CATEGORY);
            string logMessage = $"{requestingClient.nickname}'s request failed. ";

            #region post
            if (errorCategory.categoryName == LoggingConstants.CREATED_ASSIGNMENT_CATEGORY)
            {
                logMessage += "There was an error posting new assignments";
            }
            if (errorCategory.categoryName == LoggingConstants.CREATED_NEW_MESSAGE_CATEGORY)
            {
                logMessage += "There was an error creating a new message";
            }

            if (errorCategory.categoryName == LoggingConstants.CREATED_NEW_CLIENT_CATEGORY)
            {
                logMessage += "There was an error creating a new client object";
            }
            if (errorCategory.categoryName == LoggingConstants.CREATED_NEW_AUTH0_CLIENT_CATEGORY)
            {
                logMessage += "There was an error creating a new Auth0 account for a client";
            }
            if (errorCategory.categoryName == LoggingConstants.CREATED_NEW_TAG_CATEGORY)
            {
                logMessage += "There was an error creating a new tag";
            }
            if (errorCategory.categoryName == LoggingConstants.CREATED_NEW_CLIENT_TAG_RELATIONSHIPS_CATEGORY)
            {
                logMessage += "There was an error adding a tag to a client";
            }
            #endregion

            #region get
            if (errorCategory.categoryName == LoggingConstants.GET_ALL_CLIENT_CATEGORY)
            {
                logMessage += "There was an error getting all clients";
            }
            if (errorCategory.categoryName == LoggingConstants.GET_PROFILE_CATEGORY)
            {
                logMessage += "There was an error retrieving a profile";
            }
            if (errorCategory.categoryName == LoggingConstants.GET_SPECIFIC_CLIENT_CATEGORY)
            {
                logMessage += "There was an error getting the client data";
            }
            if (errorCategory.categoryName == LoggingConstants.GET_SPECIFIC_HISTORY_CATEGORY)
            {
                logMessage += "There was an error getting the history object";
            }
            if (errorCategory.categoryName == LoggingConstants.GET_ALL_HISTORY_CATEGORY)
            {
                logMessage += "There was an error getting all of the history objects";
            }
            #endregion

            #region put
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_ANSWER_CATEGORY)
            {
                logMessage += "There was an error changing the answer";
            }
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_ASSIGNMENT_STATUS_CATEGORY)
            {
                logMessage += "There was an error modifying the assignment's status";
            }
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_CLIENT_CATEGORY)
            {
                logMessage += "There was an error modifying a client";
            }
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_PROFILE_CATEGORY)
            {
                logMessage += "There was an error modifying the profile";
            }
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_MESSAGE_READ_STATUS_CATEGORY)
            {
                logMessage += "There was an error modifying the read status of a message";
            }
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_CLIENT_STATUS_CATEGORY)
            {
                logMessage += "There was an error modifying a client's status";
            }
            #endregion

            #region delete
            if (errorCategory.categoryName == LoggingConstants.DELETED_CLIENT_CATEGORY)
            {
                logMessage += "There was an error deleting a client and all the related entities";
            }
            if (errorCategory.categoryName == LoggingConstants.DELETED_ASSIGNMENT_CATEGORY)
            {
                logMessage += "There was an error deleting an assignment from a client's list";
            }
            if (errorCategory.categoryName == LoggingConstants.DELETED_TAG_CATEGORY)
            {
                logMessage += "There was an error deleting a tag";
            }
            #endregion

            await repository.CreateNewLogEntry(makeLogTemplateObject(errorCategory, logMessage));
            await saveLogs();
        }
        private Logic.Objects.Base_Objects.Logging.YuleLog makeLogTemplateObject(Category logicCategory, string logMessage)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            Logic.Objects.Base_Objects.Logging.YuleLog logicLog = new Logic.Objects.Base_Objects.Logging.YuleLog()
            {
                logID = Guid.NewGuid(),
                category = logicCategory,
                logDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone),
                logText = logMessage
            };
            return logicLog;
        }
        public async Task saveLogs()
        {
            await repository.SaveAsync();
        }
        private async Task<Category> getCategoryByName(string categoryName)
        {
            return (await repository.GetAllCategories()).First(c => c.categoryName == categoryName);
        }
        #endregion
    }
}
