using Message.Logic.Models.Auth0_Models;
using Message.Logic.Models.Common_Models;
using Message.Logic.Objects.Information_Objects;
using System;
using System.Threading.Tasks;

namespace Message.Logic.Interfaces
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