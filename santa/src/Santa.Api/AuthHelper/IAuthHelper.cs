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
        Auth0UserInfoModel getAuthClientByID(string authUserID);
        Auth0UserInfoModel getAuthClientByEmail(string authUserEmail);
        Auth0UserInfoModel changeAuthClientPassword(string authUserID, Auth0UserPasswordModel passwordModel);
    }
}
