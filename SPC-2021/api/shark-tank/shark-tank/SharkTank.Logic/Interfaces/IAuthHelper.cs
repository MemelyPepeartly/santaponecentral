using SharkTank.Logic.Models.Auth0_Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTank.Logic.Interfaces
{
    public interface IAuthHelper
    {
        #region User Info Model
        Task<Auth0UserInfoModel> createAuthClient(string authEmail, string authName);
        Task<Auth0UserInfoModel> getAuthClientByID(string authUserID);
        Task<Auth0UserInfoModel> getAuthClientByEmail(string authUserEmail);
        Task updateAuthClientRole(string authUserID, string authRoleID);
        Task updateAuthClientEmail(string authUserID, string newEmail);
        Task updateAuthClientName(string authUserID, string newName);
        Task deleteAuthClient(string authUserID);
        #endregion

        #region Roles
        /// <summary>
        /// Gets a list of all the roles available
        /// </summary>
        /// <returns></returns>
        Task<List<Auth0RoleModel>> getAllAuthRoles();
        /// <summary>
        /// Gets a role by its authRoleID
        /// </summary>
        /// <param name="authRoleID"></param>
        /// <returns></returns>
        Task<Auth0RoleModel> getAuthRole(string authRoleID);
        /// <summary>
        /// Gets a list of all the roles a client has by authUserID
        /// </summary>
        /// <param name="authUserID"></param>
        /// <returns></returns>
        Task<List<Auth0RoleModel>> getAllClientRolesByID(string authUserID);
        #endregion

        #region Tickets
        Task<Auth0TicketResponse> getPasswordChangeTicketByAuthClientEmail(string authClientEmail);
        #endregion

        #region Utility
        Task<Auth0TokenModel> getTokenModel();
        string generateTempPassword(int length);
        Task<bool> accountExists(string authUserEmail);
        #endregion
    }
}
