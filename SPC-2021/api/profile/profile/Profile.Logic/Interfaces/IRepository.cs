using Profile.Logic.Objects;
using Profile.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Logic.Interfaces
{
    public interface IRepository
    {
        #region Profile
        Task<Objects.Profile> GetProfileByEmailAsync(string email);
        Task<Objects.Profile> GetProfileByIDAsync(Guid email);
        Task<List<ProfileAssignment>> GetProfileAssignments(Guid clientID);

        #endregion

        #region Utility
        /// <summary>
        /// Saves changes of any CRUD operations in the queue
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
        #endregion
    }
}
