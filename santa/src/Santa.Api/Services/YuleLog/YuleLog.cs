using Santa.Logic.Constants;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using Santa.Logic.Objects.Base_Objects.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Services.YuleLog
{
    public class YuleLog : IYuleLog
    {
        private readonly IRepository repository;

        public YuleLog(IRepository _repository)
        {
            repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        public async Task logChangedAnswer(Client requestingClient)
        {
            throw new NotImplementedException();
        }

        public async Task logChangedAssignmentStatus(Client requestingClient)
        {
            throw new NotImplementedException();
        }

        public async Task logError(Client requestingClient)
        {
            throw new NotImplementedException();
        }

        public async Task logGetAllClients(Client requestingClient)
        {
            throw new NotImplementedException();
        }

        public async Task logGetProfile(Client requestingClient)
        {
            throw new NotImplementedException();
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
