using Santa.Api.Models.Auth0_Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.AuthHelper
{
    public interface IAuthHelper
    {
        #region Utility
        Task<Auth0TokenModel> getTokenModel();
        #endregion
        #region User Info Model
        Task<Auth0UserInfoModel> createAuthClient(string authEmail);
        Task<Auth0UserInfoModel> getAuthClientByID(string authUserID);
        Task<Auth0UserInfoModel> getAuthClientByEmail(string authUserEmail);
        Task<Auth0UserInfoModel> changeAuthClientPassword(string authUserID, Auth0UserPasswordModel passwordModel);
        #endregion
    }
}
