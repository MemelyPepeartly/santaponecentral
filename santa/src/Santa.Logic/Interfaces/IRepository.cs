using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Santa.Logic.Objects;

namespace Santa.Logic.Interfaces
{
    public interface IRepository
    {
        #region Client
        Task<Logic.Objects.Client> CreateClientAsync();
        Task<Logic.Objects.Client> GetClientByID();
        Task<Logic.Objects.Client> GetClientByEmailAsync();
        Task<Logic.Objects.Client> UpdateClientByIDAsync();
        Task<Logic.Objects.Client> DeleteClientByIDAsync();
        #endregion

        #region Event

        #endregion

        #region Survey
        #endregion
    }
}