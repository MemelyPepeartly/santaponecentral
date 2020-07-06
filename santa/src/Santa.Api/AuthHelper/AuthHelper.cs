using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Santa.Api.Models.Auth0_Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
        public Auth0TokenModel getTokenModel()
        {
            string authClientID = ConfigRoot["Auth0API:client_id"];
            string authClientSecret = ConfigRoot["Auth0API:Auth0Client_secret"];


            var tokenRestClient = new RestClient("https://memelydev.auth0.com/oauth/token");
            var tokenRequest = new RestRequest(Method.POST);
            tokenRequest.AddHeader("content-type", "application/json");
            tokenRequest.AddParameter("application/json", "{\"client_id\":\"" + authClientID + "\",\"client_secret\":\"" + authClientSecret + "\",\"audience\":\"https://memelydev.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse tokenResponse = tokenRestClient.Execute(tokenRequest);
            Auth0TokenModel token = JsonConvert.DeserializeObject<Auth0TokenModel>(tokenResponse.Content.ToString());

            return token;
        }
        public Auth0UserInfo getAuthClient(string authUserID)
        {
            var userRestClient = new RestClient("https://memelydev.auth0.com/api/v2/users/" + authUserID);
            var userRequest = new RestRequest(Method.GET);
            userRequest.AddHeader("authorization", "Bearer " + getTokenModel().access_token);
            IRestResponse response = userRestClient.Execute(userRequest);
            Auth0UserInfo user = JsonConvert.DeserializeObject<Auth0UserInfo>(response.Content.ToString());

            return user;
        }
    }
}
