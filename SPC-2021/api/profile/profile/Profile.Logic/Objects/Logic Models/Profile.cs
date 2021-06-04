using Profile.Logic.Models.Common_Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Profile.Logic.Objects
{
    public class Profile
    {
        public Guid clientID { get; set; }
        public string clientName { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
        public List<Response> responses { get; set; }
        public bool editable { get; set; }
        public SharkTankValidationResponseModel sharkTankValidationResponse { get; set; }
    }
}
