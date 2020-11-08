using Santa.Api.Models.Assignment_Status_Models;
using Santa.Data.Entities;
using Santa.Logic.Constants;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using Santa.Logic.Objects.Base_Objects.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AssignmentStatus = Santa.Logic.Objects.AssignmentStatus;
using Category = Santa.Logic.Objects.Base_Objects.Logging.Category;
using Client = Santa.Logic.Objects.Client;

namespace Santa.Api.Services.YuleLog
{
    public class YuleLog : IYuleLog
    {
        private readonly IRepository repository;

        public YuleLog(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        #region POST logs
        public async Task logCreatedNewAssignments(Client requestingClient, Client sendingClient, List<string> listNewAssignmentNicknames)
        {
            string logMessage = $"{requestingClient.nickname} made a request to add the following assignments to {sendingClient.nickname}: ";
            foreach(string assignmentNickname in listNewAssignmentNicknames)
            {
                logMessage += assignmentNickname;
                if (!listNewAssignmentNicknames.IndexOf(assignmentNickname).Equals(listNewAssignmentNicknames.Count -1))
                {
                    logMessage += ", ";
                }
            }

            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.POSTED_ASSIGNMENT_CATEGORY), logMessage));
            await saveLogs();
        }
        #endregion

        #region GET logs
        public async Task logGetAllClients(Client requestingClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_ALL_CLIENT_CATEGORY), requestingClient.nickname + " made a request to retrieve a list of all clients"));
            await saveLogs();
        }
        public async Task logGetSpecificClient(Client requestingClient, Client requestedClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_SPECIFIC_CLIENT_CATEGORY), requestingClient.nickname + " made a request to retrieve the client information for " + requestedClient.nickname));
            await saveLogs();
        }
        public async Task logGetProfile(Client requestingClient, Profile returnedProfile)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_PROFILE_CATEGORY), requestingClient.nickname + " made a request to get a profile object for: " + returnedProfile.nickname));
            await saveLogs();
        }
        public async Task logGetAllHistories(Client requestingClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_ALL_HISTORY_CATEGORY), requestingClient.nickname + " made a request to retrieve a list of all chat histories"));
            await saveLogs();
        }
        public async Task logGetSpecificHistory(Client requestingClient, MessageHistory requestedHistory)
        {
            string logMessage = requestedHistory.relationXrefID != null ? $"{requestingClient.nickname} requested to get a history between admins and {requestedHistory.conversationClient.clientNickname} about an assignment ({requestedHistory.assignmentRecieverClient.clientNickname}) " : $"{requestingClient.nickname} requested to get a general history between admins and {requestedHistory.conversationClient.clientNickname}";
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_SPECIFIC_HISTORY_CATEGORY), logMessage));
            await saveLogs();
        }
        #endregion

        #region PUT logs
        public async Task logChangedAnswer(Client requestingClient, Question questionBeingAnsweredFor, string oldAnswer, string newAnswer)
        {
            string logMessage = $@"{requestingClient.nickname} made a request to change their answer for question '{questionBeingAnsweredFor.questionText}' from '{oldAnswer}' to '{newAnswer}'";
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.MODIFIED_ANSWER_CATEGORY), logMessage));
            await saveLogs();
        }
        public async Task logChangedAssignmentStatus(Client requestingClient, string assignmentNickname, AssignmentStatus oldStatus, AssignmentStatus newStatus)
        {
            string logMessage = $@"{requestingClient.nickname} made a request to change the assignment status for {assignmentNickname} from '{oldStatus.assignmentStatusName}' to '{newStatus.assignmentStatusName}'";
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.MODIFIED_ASSIGNMENT_STATUS_CATEGORY), logMessage));
            await saveLogs();
        }
        public async Task logChangedProfile(Client requestingClient, Profile modifiedProfile)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_ALL_HISTORY_CATEGORY), $"{requestingClient.nickname} modified data on a profile for {modifiedProfile.nickname}"));
            await saveLogs();
        }
        public async Task logChangedClient(Client requestingClient, Client modifiedClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_ALL_HISTORY_CATEGORY), $"{requestingClient.nickname} modified a client's data for {modifiedClient.nickname}"));
            await saveLogs();
        }
        #endregion

        #region DELETE logs
        #endregion

        #region Utility
        public async Task logError(Client requestingClient, string category)
        {
            Category errorCategory = await getCategoryByName(LoggingConstants.MODIFIED_ANSWER_CATEGORY);
            string logMessage = "";
            #region post
            if (errorCategory.categoryName == LoggingConstants.POSTED_ASSIGNMENT_CATEGORY)
            {
                logMessage = "There was an error posting new assignments";
            }
            #endregion

            #region get
            if (errorCategory.categoryName == LoggingConstants.GET_ALL_CLIENT_CATEGORY)
            {
                logMessage = "There was an error getting all clients";
            }
            if (errorCategory.categoryName == LoggingConstants.GET_PROFILE_CATEGORY)
            {
                logMessage = "There was an error retrieving a profile";
            }
            if (errorCategory.categoryName == LoggingConstants.GET_SPECIFIC_CLIENT_CATEGORY)
            {
                logMessage = "There was an error getting the client data";
            }
            if (errorCategory.categoryName == LoggingConstants.GET_SPECIFIC_HISTORY_CATEGORY)
            {
                logMessage = "There was an error getting the history object";
            }
            if (errorCategory.categoryName == LoggingConstants.GET_ALL_HISTORY_CATEGORY)
            {
                logMessage = "There was an error getting all of the history objects";
            }
            #endregion

            #region put
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_ANSWER_CATEGORY)
            {
                logMessage = "There was an error changing the answer";
            }
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_ASSIGNMENT_STATUS_CATEGORY)
            {
                logMessage = "There was an error modifying the assignment's status";
            }
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_CLIENT_CATEGORY)
            {
                logMessage = "There was an error modifying a client";
            }
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_PROFILE_CATEGORY)
            {
                logMessage = "There was an error modifying the profile";
            }
            #endregion

            #region delete
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
