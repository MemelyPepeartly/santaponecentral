using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Logic.Models.Auth0_Response_Models
{
    public class Auth0InfoRequestModel
    {
        /// <summary>
        /// The ID of the client making the request for information
        /// </summary>
        public Guid? requestingClientID { get; set; }
        /// <summary>
        /// The email of the client making the request for information
        /// </summary>
        public string? requestingClientEmail { get; set; }
        /// <summary>
        /// Requested clientID of the information the requester wants to access
        /// </summary>
        public Guid? requestedClientID { get; set; }
        /// <summary>
        /// Requested email of the information the requester wants to access
        /// </summary>
        public string? requestedClientEmail { get; set; }
    }
}
