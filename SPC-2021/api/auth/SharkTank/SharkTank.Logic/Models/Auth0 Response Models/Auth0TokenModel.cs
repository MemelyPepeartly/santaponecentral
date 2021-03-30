using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharkTank.Logic.Models.Auth0_Response_Models
{
    public class Auth0TokenModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }
}
