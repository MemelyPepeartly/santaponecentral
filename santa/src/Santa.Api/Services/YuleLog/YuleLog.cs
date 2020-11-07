using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
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

        public Task logChangedAnswer(Client requestingClient)
        {
            throw new NotImplementedException();
        }

        public Task logChangedAssignmentStatus(Client requestingClient)
        {
            throw new NotImplementedException();
        }

        public Task logError(Client requestingClient)
        {
            throw new NotImplementedException();
        }

        public Task logGetAllClients(Client requestingClient)
        {
            throw new NotImplementedException();
        }

        public Task logGetProfile(Client requestingClient)
        {
            throw new NotImplementedException();
        }
    }
}
