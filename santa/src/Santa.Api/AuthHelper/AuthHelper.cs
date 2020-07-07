using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Santa.Api.Models.Auth0_Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.AuthHelper
{
    public class AuthHelper : IAuthHelper
    {
        private IConfigurationRoot ConfigRoot;

        public AuthHelper(IConfiguration configRoot)
        {
            ConfigRoot = (IConfigurationRoot)configRoot;
        }
        public async Task<Auth0TokenModel> getTokenModel()
        {
            string authClientID = ConfigRoot["Auth0API:client_id"];
            string authClientSecret = ConfigRoot["Auth0API:Auth0Client_secret"];


            RestClient tokenRestClient = new RestClient("https://memelydev.auth0.com/oauth/token");
            RestRequest tokenRequest = new RestRequest(Method.POST);
            tokenRequest.AddHeader("content-type", "application/json");
            tokenRequest.AddParameter("application/json", "{\"client_id\":\"" + authClientID + "\",\"client_secret\":\"" + authClientSecret + "\",\"audience\":\"https://memelydev.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse tokenResponse = await tokenRestClient.ExecuteAsync(tokenRequest);
            Auth0TokenModel token = JsonConvert.DeserializeObject<Auth0TokenModel>(tokenResponse.Content.ToString());

            return token;
        }
        public async Task<Auth0UserInfoModel> getAuthClientByID(string authUserID)
        {
            RestClient userRestClient = new RestClient("https://memelydev.auth0.com/api/v2/users/" + authUserID);
            RestRequest userRequest = new RestRequest(Method.GET);
            Auth0TokenModel token = await getTokenModel();
            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            Auth0UserInfoModel user = JsonConvert.DeserializeObject<Auth0UserInfoModel>(response.Content.ToString());

            return user;
        }

        public async Task<Auth0UserInfoModel> getAuthClientByEmail(string authUserEmail)
        {
            RestClient userRestClient = new RestClient("https://memelydev.auth0.com/api/v2/users-by-email?email=" + authUserEmail);
            RestRequest userRequest = new RestRequest(Method.GET);
            Auth0TokenModel token = await getTokenModel();
            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            Auth0UserInfoModel user = JsonConvert.DeserializeObject<Auth0UserInfoModel>(response.Content.ToString());

            return user;
        }

        public async Task<Auth0UserInfoModel> changeAuthClientPassword(string authUserID, Auth0UserPasswordModel passwordModel)
        {
            RestClient userRestClient = new RestClient("https://memelydev.auth0.com/api/v2/users/" + authUserID);
            RestRequest userRequest = new RestRequest(Method.POST);
            Auth0TokenModel token = await getTokenModel();
            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            userRequest.AddJsonBody(passwordModel);
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            Auth0UserInfoModel user = JsonConvert.DeserializeObject<Auth0UserInfoModel>(response.Content.ToString());

            return user;

        }

        public async Task<Auth0UserInfoModel> createAuthClient(string authEmail)
        {
            RestClient userRestClient = new RestClient("https://memelydev.auth0.com/api/v2/users");
            RestRequest userRequest = new RestRequest(Method.POST);
            Auth0TokenModel token = await getTokenModel();
            userRequest.AddHeader("authorization", "Bearer " + token.access_token);
            userRequest.AddJsonBody(new Auth0NewUserModel()
            {
                email = authEmail,
                name = authEmail,
                connection = "Username-Password-Authentication",
                password = "TestPass2244",
                verify_email = true
            });
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            Auth0UserInfoModel user = JsonConvert.DeserializeObject<Auth0UserInfoModel>(response.Content.ToString());

            return user;
        }
    }
}
