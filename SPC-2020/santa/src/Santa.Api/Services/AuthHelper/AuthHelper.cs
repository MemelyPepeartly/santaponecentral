using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Santa.Api.Models.Auth0_Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Santa.Api.AuthHelper
{
    public class AuthHelper : IAuthHelper
    {
        private IConfigurationRoot ConfigRoot;
        private string endpoint = String.Empty;

        public AuthHelper(IConfiguration configRoot)
        {
            ConfigRoot = (IConfigurationRoot)configRoot;
            endpoint = ConfigRoot["Auth0API:endpoint"];
        }

        #region User Info Model
        public async Task<Auth0UserInfoModel> createAuthClient(string authEmail, string authName)
        {
            RestClient userRestClient = new RestClient(endpoint + "users");
            RestRequest userRequest = new RestRequest(Method.POST);
            Auth0TokenModel token = await getTokenModel();
            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            Auth0NewUserModel newUser = new Auth0NewUserModel()
            {
                email = authEmail,
                name = authName,
                connection = "Username-Password-Authentication",
                password = generateTempPassword(),
                verify_email = false
            };
            userRequest.AddJsonBody(newUser);
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            Auth0UserInfoModel user = JsonConvert.DeserializeObject<Auth0UserInfoModel>(response.Content);
            return user;
        }
        public async Task<Auth0UserInfoModel> getAuthClientByID(string authUserID)
        {
            RestClient userRestClient = new RestClient(endpoint + "users/" + authUserID);
            RestRequest userRequest = new RestRequest(Method.GET);
            Auth0TokenModel token = await getTokenModel();
            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            Auth0UserInfoModel user = JsonConvert.DeserializeObject<Auth0UserInfoModel>(response.Content);

            return user;
        }

        public async Task<Auth0UserInfoModel> getAuthClientByEmail(string authUserEmail)
        {
            RestClient userRestClient = new RestClient(endpoint + "users-by-email?email=" + authUserEmail);
            RestRequest userRequest = new RestRequest(Method.GET);
            Auth0TokenModel token = await getTokenModel();
            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            List<Auth0UserInfoModel> users = JsonConvert.DeserializeObject<List<Auth0UserInfoModel>>(response.Content);

            return users.First(c => c.email == authUserEmail);
        }
        public async Task updateAuthClientRole(string authUserID, string authRoleID)
        {
            RestClient userRestClient = new RestClient(endpoint + "users/" + authUserID + "/roles");
            RestRequest userRequest = new RestRequest(Method.POST);
            Auth0TokenModel token = await getTokenModel();

            Auth0AddRoleIDModel responseModel = new Auth0AddRoleIDModel()
            {
                roles = new List<string>()
            };
            responseModel.roles.Add(authRoleID);

            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            userRequest.AddJsonBody(responseModel);
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);

            if(response.IsSuccessful)
            {
                return;
            }
            else
            {
                throw new Exception();
            }

        }
        public async Task updateAuthClientEmail(string authUserID, string newEmail)
        {
            RestClient userRestClient = new RestClient(endpoint + "users/" + authUserID);
            RestRequest userRequest = new RestRequest(Method.PATCH);
            Auth0TokenModel token = await getTokenModel();

            var responseModel = new Models.Auth0_Response_Models.Auth0ChangeEmailModel()
            {
                email = newEmail,
            };

            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            userRequest.AddJsonBody(responseModel);
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);

            if (response.IsSuccessful)
            {
                return;
            }
            else
            {
                throw new Exception();
            }
        }
        public async Task updateAuthClientName(string authUserID, string newName)
        {
            RestClient userRestClient = new RestClient(endpoint + "users/" + authUserID);
            RestRequest userRequest = new RestRequest(Method.PATCH);
            Auth0TokenModel token = await getTokenModel();

            var responseModel = new Models.Auth0_Response_Models.Auth0ChangeNameModel()
            {
                name = newName,
            };

            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            userRequest.AddJsonBody(responseModel);
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);

            if (response.IsSuccessful)
            {
                return;
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task deleteAuthClient(string authUserID)
        {
            RestClient userRestClient = new RestClient(endpoint + "users/" + authUserID);
            RestRequest userRequest = new RestRequest(Method.DELETE);
            Auth0TokenModel token = await getTokenModel();

            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);

            if(response.IsSuccessful)
            {
                return;
            }
            else
            {
                throw new Exception();
            }
        }
        #endregion

        #region Roles
        public async Task<List<Auth0RoleModel>> getAllAuthRoles()
        {
            RestClient roleRestClient = new RestClient(endpoint + "roles");
            RestRequest roleRequest = new RestRequest(Method.GET);
            Auth0TokenModel token = await getTokenModel();
            roleRequest.AddHeader("authorization", "Bearer " + token.access_token);
            IRestResponse response = await roleRestClient.ExecuteAsync(roleRequest);

            List<Auth0RoleModel> roles = JsonConvert.DeserializeObject<List<Auth0RoleModel>>(response.Content);

            return roles;
        }

        public async Task<Auth0RoleModel> getAuthRole(string authRoleID)
        {
            RestClient roleRestClient = new RestClient(endpoint + "roles/" + authRoleID);
            RestRequest roleRequest = new RestRequest(Method.GET);
            Auth0TokenModel token = await getTokenModel();
            roleRequest.AddHeader("authorization", "Bearer " + token.access_token);
            IRestResponse response = await roleRestClient.ExecuteAsync(roleRequest);
            Auth0RoleModel role = JsonConvert.DeserializeObject<Auth0RoleModel>(response.Content);

            return role;

        }
        #endregion

        #region Tickets
        public async Task<Auth0TicketResponse> getPasswordChangeTicketByAuthClientEmail(string authClientEmail)
        {
            RestClient roleRestClient = new RestClient(endpoint + "tickets/password-change");
            RestRequest roleRequest = new RestRequest(Method.POST);
            Auth0TokenModel token = await getTokenModel();
            roleRequest.AddHeader("authorization", "Bearer " + token.access_token);
            roleRequest.AddJsonBody(new Auth0ChangePasswordModel()
            {
                email = authClientEmail,
                connection_id = ConfigRoot["Auth0API:ConnectionID"],
                mark_email_as_verified = true,
                includeEmailInRedirect = true
            });
            IRestResponse response = await roleRestClient.ExecuteAsync(roleRequest);
            Auth0TicketResponse ticket = JsonConvert.DeserializeObject<Auth0TicketResponse>(response.Content);

            return ticket;

        }
        #endregion

        #region Utility
        public async Task<Auth0TokenModel> getTokenModel()
        {
            string authClientID = ConfigRoot["Auth0API:client_id"];
            string authClientSecret = ConfigRoot["Auth0API:Auth0Client_secret"];
            string authClientAudience = ConfigRoot["Auth0API:authServiceAudience"];
            string tokenURIPath = ConfigRoot["Auth0API:tokenURIPath"];

            RestClient tokenRestClient = new RestClient(tokenURIPath);
            RestRequest tokenRequest = new RestRequest(Method.POST);
            tokenRequest.AddHeader("content-type", "application/json");
            tokenRequest.AddParameter("application/json", "{\"client_id\":\"" + authClientID + "\",\"client_secret\":\"" + authClientSecret + "\",\"audience\":\""+ authClientAudience + "\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse tokenResponse = await tokenRestClient.ExecuteAsync(tokenRequest);
            Auth0TokenModel token = JsonConvert.DeserializeObject<Auth0TokenModel>(tokenResponse.Content);

            return token;
        }

        public string generateTempPassword(int length = 10)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        public async Task<bool> accountExists(string authUserEmail)
        {
            RestClient userRestClient = new RestClient(endpoint + "users-by-email?email=" + authUserEmail);
            RestRequest userRequest = new RestRequest(Method.GET);
            Auth0TokenModel token = await getTokenModel();
            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            List<Auth0UserInfoModel> users = JsonConvert.DeserializeObject<List<Auth0UserInfoModel>>(response.Content);

            return users.Count > 0 && !string.IsNullOrEmpty(users.First(c => c.email == authUserEmail).user_id);
        }
        #endregion
    }
}
