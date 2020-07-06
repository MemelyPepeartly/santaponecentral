using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Auth0_Response_Models
{
    public class Auth0UserInfo
    {
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string email { get; set; }
        public string email_verified { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }
        public string user_id { get; set; }
    }
}
