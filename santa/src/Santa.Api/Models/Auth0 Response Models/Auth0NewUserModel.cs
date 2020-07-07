using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Auth0_Response_Models
{
    public class Auth0NewUserModel
    {
        public string email { get; set; }
        public string name { get; set; }
        public string connection { get; set; }
        public string password { get; set; }
        public bool verify_email { get; set; }
    }
}
