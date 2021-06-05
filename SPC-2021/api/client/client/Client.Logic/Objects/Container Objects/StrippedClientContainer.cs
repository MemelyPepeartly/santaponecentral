using Client.Logic.Models.Common_Models;
using Client.Logic.Objects.Information_Objects;
using System.Collections.Generic;

namespace Client.Logic.Objects.Container_Objects
{
    public class StrippedClientContainer
    {
        public List<StrippedClient> strippedClients { get; set; }
        public SharkTankValidationResponseModel sharkTankValidationResponse { get; set; }
    }
}
