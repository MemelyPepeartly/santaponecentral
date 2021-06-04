using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SharkTank.Logic.Models.Common_Models
{
    public class SharkTankValidationModel
    {
        /// <summary>
        /// Client requesting the object
        /// </summary>
        public Guid requestorClientID { get; set; }
        /// <summary>
        /// Roles the requestor has
        /// </summary>
        public List<Claim> requestorRoles { get; set; }
        /// <summary>
        /// Object category for logging and sharkTank purposes
        /// </summary>
        public string requestedObjectCategory { get; set; }
        /// <summary>
        /// ID needed to validate against. Can be for any object, so long as it is a Guid
        /// </summary>
        public Guid? validationID { get; set; }
        /// <summary>
        /// HTTP method used in this transaction (GET, POST, PUT, DELETE)
        /// </summary>
        public Method httpMethod { get; set; }
    }
}