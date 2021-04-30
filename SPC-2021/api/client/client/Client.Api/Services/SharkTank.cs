using Client.Logic.Interfaces;
using Client.Logic.Models.Auth0_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Api.Services
{
    public class SharkTank : ISharkTank
    {
        public async Task<object> CheckIfValidRequest()
        {
            throw new NotImplementedException();
        }

        public async Task<object> DeleteAuthUser()
        {
            throw new NotImplementedException();
        }

        public async Task<Auth0UserInfoModel> GetAuthInfo()
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
