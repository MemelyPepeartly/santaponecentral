using Santa.Api.Models.Assignment_Status_Models;
using Santa.Data.Entities;
using Santa.Logic.Constants;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using Santa.Logic.Objects.Base_Objects.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task logChangedAnswer(Client requestingClient, Question questionBeingAnsweredFor, Response oldAnswer, Response newAnswer)
        {
            string logMessage = $@"{requestingClient.nickname} made a request to change their answer for question '{questionBeingAnsweredFor.questionText}' from '{oldAnswer.responseText}' to '{newAnswer.responseText}'";
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.MODIFIED_ANSWER_CATEGORY), logMessage));
        }

        public async Task logChangedAssignmentStatus(Client requestingClient, string assignmentNickname, AssignmentStatus oldStatus, AssignmentStatus newStatus)
        {
            string logMessage = $@"{requestingClient.nickname} made a request to change the assignment status for {assignmentNickname} from '{oldStatus.assignmentStatusName}' to '{newStatus.assignmentStatusName}'";
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.MODIFIED_ASSIGNMENT_STATUS_CATEGORY), logMessage));
        }

        public async Task logError(Client requestingClient, string category)
        {
            Category errorCategory = await getCategoryByName(LoggingConstants.MODIFIED_ANSWER_CATEGORY);
            string logMessage = "";
            if(errorCategory.categoryName == LoggingConstants.GET_ALL_CLIENT_CATEGORY)
            {
                logMessage = "There was an error getting all clients";
            }
            if(errorCategory.categoryName == LoggingConstants.GET_PROFILE_CATEGORY)
            {
                logMessage = "There was an error retrieving a profile";
            }
            if (errorCategory.categoryName == LoggingConstants.GET_SPECIFIC_CLIENT_CATEGORY)
            {
                logMessage = "There was an error getting the client data";
            }
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_ANSWER_CATEGORY)
            {
                logMessage = "There was an error changing the answer";
            }
            if (errorCategory.categoryName == LoggingConstants.MODIFIED_ASSIGNMENT_STATUS_CATEGORY)
            {
                logMessage = "There was an error modifying the assignment's status";
            }
            await repository.CreateNewLogEntry(makeLogTemplateObject(errorCategory, logMessage));
        }

        public async Task logGetAllClients(Client requestingClient)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_ALL_CLIENT_CATEGORY), requestingClient.nickname + " made a request to retrieve a list of all clients"));
        }

        public async Task logGetProfile(Client requestingClient, Profile returnedProfile)
        {
            await repository.CreateNewLogEntry(makeLogTemplateObject(await getCategoryByName(LoggingConstants.GET_PROFILE_CATEGORY), requestingClient.nickname + " made a request to get a profile for: " + returnedProfile.nickname));
        }
        private Logic.Objects.Base_Objects.Logging.YuleLog makeLogTemplateObject(Category logicCategory, string logMessage)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            Logic.Objects.Base_Objects.Logging.YuleLog logicLog = new Logic.Objects.Base_Objects.Logging.YuleLog()
            {
                logID = Guid.NewGuid(),
                category = logicCategory,
                logDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone),
                logtext = logMessage
            };
            return logicLog;
        }
        private async Task<Category> getCategoryByName(string categoryName)
        {
            return (await repository.GetAllCategories()).First(c => c.categoryName == categoryName);
        }
    }
}
