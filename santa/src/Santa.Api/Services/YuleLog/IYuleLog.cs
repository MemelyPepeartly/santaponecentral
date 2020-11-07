using Santa.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Services.YuleLog
{
    public interface IYuleLog
    {
        Task logGetAllClients(Client requestingClient);
        Task logGetProfile(Client requestingClient);
        Task logChangedAnswer(Client requestingClient);
        Task logChangedAssignmentStatus(Client requestingClient);
        Task logError(Client requestingClient);
    }
}
