using Client.Logic.Models.Auth0_Models;
using Client.Logic.Models.Common_Models;
using Client.Logic.Objects.Information_Objects;
using System;
using System.Threading.Tasks;

namespace Client.Logic.Interfaces
{
    public interface ISharkTank
    {
        Task<Auth0UserInfoModel> GetAuthInfo();
        Task<object> PostNewAuthUser();
        Task<SharkTankValidationResponseModel> CheckIfValidRequest(SharkTankValidationModel requestModel);
        Task<object> PutAuthEmail(Guid clientID);
        Task<object> PutAuthName(Guid clientID);
        Task<object> PutPasswordTicketRequest(Guid clientID);
        Task<object> DeleteAuthUser(Guid clientID);
        Task<object> MakeNewSuccessLog(BaseClient requestingClient, string category);
        Task<object> MakeNewFailiureLog(BaseClient requestingClient, string category);
    }
}