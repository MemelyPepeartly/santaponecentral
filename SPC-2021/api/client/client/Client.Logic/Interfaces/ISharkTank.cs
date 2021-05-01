using Client.Logic.Models.Auth0_Models;
using Client.Logic.Objects.Information_Objects;
using System.Threading.Tasks;

namespace Client.Logic.Interfaces
{
    public interface ISharkTank
    {
        Task<Auth0UserInfoModel> GetAuthInfo();
        Task<object> PostNewAuthUser();
        Task<object> CheckIfValidRequest();
        Task<object> PutEmail();
        Task<object> PutName();
        Task<object> PutPasswordTicketRequest();
        Task<object> DeleteAuthUser();
        Task<object> MakeNewSuccessLog(BaseClient requestingClient, string category);
        Task<object> MakeNewFailiureLog(BaseClient requestingClient, string category);
    }
}
s