using System;

namespace Profile.Logic.Models.Auth0_Response_Models
{
    public class Auth0CreateNewUserRequestModel
    {
        public Guid clientID { get; set; }
        public string nickname { get; set; }
        public string clientEmail { get; set; }
    }
}
