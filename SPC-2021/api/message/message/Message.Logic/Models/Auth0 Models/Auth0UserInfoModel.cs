using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Message.Logic.Models.Auth0_Models
{
    public class Auth0UserInfoModel
    {
        public DateTime created_at { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        public List<Auth0IdentityModel> identities { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }
        public string picture { get; set; }
        public DateTime updated_at { get; set; }
        public string user_id { get; set; }
        public DateTime last_password_reset { get; set; }
        public string last_ip { get; set; }
        public DateTime last_login { get; set; }
        public int logins_count { get; set; }

    }
}
