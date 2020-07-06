using Santa.Api.Models.Auth0_Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.AuthHelper
{
    public interface IAuthHelper
    {
        Auth0TokenModel getTokenModel();
        Auth0UserInfo getAuthClient(string authUserID);
    }
}
