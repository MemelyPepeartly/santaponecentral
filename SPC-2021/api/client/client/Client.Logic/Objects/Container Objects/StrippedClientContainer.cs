using Client.Logic.Models.Common_Models;
using Client.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic.Objects.Container_Objects
{
    public class StrippedClientContainer
    {
        public List<StrippedClient> strippedClients { get; set; }
        public SharkTankValidationResponseModel sharkTankValidationResponse { get; set; }
    }
}
