using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Logic.Models.Auth0_Response_Models
{
    public class Auth0CreateNewUserRequestModel
    {
        public Guid clientID { get; set; }
        public string nickname { get; set; }
        public string clientEmail { get; set; }
    }
}
