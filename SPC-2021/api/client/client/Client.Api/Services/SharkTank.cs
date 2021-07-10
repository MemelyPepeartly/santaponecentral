using Client.Logic.Interfaces;
using Client.Logic.Models.Auth0_Models;
using Client.Logic.Models.Common_Models;
using Client.Logic.Objects.Information_Objects;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
        public async Task<SharkTankValidationResponseModel> CheckIfValidRequest(SharkTankValidationModel requestModel)
        {
            // Setup rest client
            RestClient userRestClient = new RestClient(endpoint + "SharkTank/Validate");
            RestRequest userRequest = new RestRequest(Method.POST);
            // New request object for body
            userRequest.AddJsonBody(requestModel);
            // HTTP Request
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            if(response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<SharkTankValidationResponseModel>(response.Content);
            }
            else
            {
                throw response.ErrorException;
            }
        }

        public async Task<object> DeleteAuthUser(string authClientID)
        {
#warning double check this endpoint to see if it is better to use authclientID or normal clientID
            // Setup rest client
            RestClient userRestClient = new RestClient($"{endpoint}SharkTank/{authClientID}");
            RestRequest userRequest = new RestRequest(Method.DELETE);
            // New request object for body
            object request = new object();
            userRequest.AddJsonBody(request);
            // HTTP Request
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            return response;
        }

        public async Task<Auth0UserInfoModel> GetAuthInfo(string authEmail)
        {
            // Setup rest client
            RestClient userRestClient = new RestClient(endpoint + "SharkTank/AuthInfo");
            RestRequest userRequest = new RestRequest(Method.POST);
            // New request object for body
            object request = new object();
            userRequest.AddJsonBody(request);
            // HTTP Request
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            return JsonConvert.DeserializeObject<Auth0UserInfoModel>(response.Content);
        }

        public async Task<object> MakeNewFailureLog(BaseClient requestingClient, string category)
        {
            // Setup rest client
            RestClient userRestClient = new RestClient($"{endpoint}SharkTank/NewLog");
            RestRequest userRequest = new RestRequest(Method.POST);
            // New request object for body
            object request = new object();
            userRequest.AddJsonBody(request);
            // HTTP Request
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            return JsonConvert.DeserializeObject<object>(response.Content);
        }

        public async Task<object> MakeNewLog()
        {
            // Setup rest client
            RestClient userRestClient = new RestClient($"{endpoint}SharkTank/NewLog");
            RestRequest userRequest = new RestRequest(Method.POST);
            // New request object for body
            object request = new object();
            userRequest.AddJsonBody(request);
            // HTTP Request
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            return JsonConvert.DeserializeObject<object>(response.Content);
        }

        public async Task<object> MakeNewSuccessLog(BaseClient requestingClient, string category)
        {
            // Setup rest client
            RestClient userRestClient = new RestClient($"{endpoint}SharkTank/NewLog");
            RestRequest userRequest = new RestRequest(Method.POST);
            // New request object for body
            object request = new object();
            userRequest.AddJsonBody(request);
            // HTTP Request
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            return JsonConvert.DeserializeObject<object>(response.Content);
        }

        public async Task<object> PostNewAuthUser()
        {
            // Setup rest client
            RestClient userRestClient = new RestClient(endpoint + "SharkTank");
            RestRequest userRequest = new RestRequest(Method.POST);
            // New request object for body
            object request = new object();
            userRequest.AddJsonBody(request);
            // HTTP Request
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            return JsonConvert.DeserializeObject<Auth0UserInfoModel>(response.Content);
        }

        public async Task<object> PutAuthEmail(Guid clientID)
        {
            // Setup rest client
            RestClient userRestClient = new RestClient($"{endpoint}SharkTank/{clientID}/Email");
            RestRequest userRequest = new RestRequest(Method.PUT);
            // New request object for body
            object request = new object();
            userRequest.AddJsonBody(request);
            // HTTP Request
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            return JsonConvert.DeserializeObject<Auth0UserInfoModel>(response.Content);
        }

        public async Task<object> PutAuthName(Guid clientID)
        {
            // Setup rest client
            RestClient userRestClient = new RestClient($"{endpoint}SharkTank/{clientID}/Name");
            RestRequest userRequest = new RestRequest(Method.PUT);
            // New request object for body
            object request = new object();
            userRequest.AddJsonBody(request);
            // HTTP Request
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            return JsonConvert.DeserializeObject<Auth0UserInfoModel>(response.Content);
        }

        public async Task<object> PutPasswordTicketRequest(Guid clientID)
        {
            // Setup rest client
            RestClient userRestClient = new RestClient($"{endpoint}SharkTank/{clientID}/Password");
            RestRequest userRequest = new RestRequest(Method.PUT);
            // New request object for body
            object request = new object();
            userRequest.AddJsonBody(request);
            // HTTP Request
            IRestResponse response = await userRestClient.ExecuteAsync(userRequest);
            return JsonConvert.DeserializeObject<object>(response.Content);
        }
    }
}
