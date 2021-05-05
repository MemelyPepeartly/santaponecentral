using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTank.Logic.Models.Common_Models
{
    public class SharkTankValidationModel
    {
        public Guid? requestorClientID { get; set; }
        public Guid? requestObject { get; set; }
        public List<string> requestorRoles { get; set; }
        public Method httpMethod { get; set; }
    }
}