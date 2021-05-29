using Profile.Logic.Models.Auth0_Response_Models;
using Profile.Logic.Models.Common_Models;
using Profile.Logic.Objects.Information_Objects;
using System;
using System.Threading.Tasks;

namespace Profile.Logic.Interfaces
{
    public interface ISharkTank
    {
        Task<Auth0UserInfoModel> GetAuthInfo(string authEmail);
        Task<object> PostNewAuthUser();
        Task<SharkTankValidationResponseModel> CheckIfValidRequest(SharkTankValidationModel requestModel);
        Task<object> PutAuthEmail(Guid clientID);
        Task<object> PutAuthName(Guid clientID);
        Task<object> PutPasswordTicketRequest(Guid clientID);
        Task<object> DeleteAuthUser(string authClientID);
        Task<object> MakeNewSuccessLog(BaseClient requestingClient, string category);
        Task<object> MakeNewFailureLog(BaseClient requestingClient, string category);
    }
}