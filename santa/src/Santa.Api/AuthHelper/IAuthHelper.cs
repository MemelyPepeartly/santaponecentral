﻿using Santa.Api.Models.Auth0_Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.AuthHelper
{
    public interface IAuthHelper
    {
        #region User Info Model
        Task<Auth0UserInfoModel> createAuthClient(string authEmail);
        Task<Auth0UserInfoModel> getAuthClientByID(string authUserID);
        Task<Auth0UserInfoModel> getAuthClientByEmail(string authUserEmail);
        Task<Auth0UserInfoModel> updateAuthClientPassword(string authUserID, Auth0UserPasswordModel passwordModel);
        Task updateAuthClientRole(string authUserID, string authRoleID);
        #endregion

        #region Roles
        Task<List<Auth0RoleModel>> getAllAuthRoles();
        Task<Auth0RoleModel> getAuthRole(string authRoleID);
        #endregion

        #region Tickets
        Task<Auth0TicketResponse> triggerPasswordChangeNotification(string authClientEmail);
        #endregion

        #region Utility
        Task<Auth0TokenModel> getTokenModel();
        string generateTempPassword(int length);
        #endregion
    }
}