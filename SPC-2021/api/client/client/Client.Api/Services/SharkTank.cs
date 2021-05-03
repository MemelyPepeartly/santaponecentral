using Client.Logic.Interfaces;
using Client.Logic.Models.Auth0_Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Api.Services
{
    public class SharkTank : ISharkTank
    {
        private IConfigurationRoot ConfigRoot;
        private string endpoint = String.Empty;

        public SharkTank(IConfiguration configRoot)
        {
            ConfigRoot = (IConfigurationRoot)configRoot;
            endpoint = ConfigRoot["sharkTankAPIEndpoint"];
        }
        public async Task<object> CheckIfValidRequest()
        {
            // Setup rest client
            RestClient userRestClient = new RestClient(endpoint + "SharkTank/Validate");
            RestRequest userRequest = new RestRequest(Method.POST);
            // New request object for body
            object request = new object();
            userRequest.AddJsonBody(request);
            // HTTP Request
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            return response;
        }

        public async Task<object> DeleteAuthUser()
        {
            throw new NotImplementedException();
        }

        public async Task<Auth0UserInfoModel> GetAuthInfo()
        {
            throw new NotImplementedException();
        }

        public Task<object> MakeNewLog()
        {
            throw new NotImplementedException();
        }

        public async Task<object> PostNewAuthUser()
        {
            throw new NotImplementedException();
        }

        public async Task<object> PutEmail()
        {
            throw new NotImplementedException();
        }

        public async Task<object> PutName()
        {
            throw new NotImplementedException();
        }

        public async Task<object> PutPasswordTicketRequest()
        {
            throw new NotImplementedException();
        }
    }
}
