using Client.Logic.Models.Auth0_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
