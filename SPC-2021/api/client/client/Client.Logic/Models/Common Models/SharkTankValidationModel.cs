using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic.Models.Common_Models
{
    public class SharkTankValidationModel
    {
        public Guid requestorClientID { get; set; }
        public List<Claim> requestorRoles { get; set; }
        public string requestedObjectCategory { get; set; }
        public Guid? requestedObjectID { get; set; }
        public Method httpMethod { get; set; }
    }
}