using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Search.Logic.Objects;
using Search.Logic.Objects.Base_Objects;
using Search.Logic.Objects.Base_Objects.Logging;
using Search.Logic.Objects.Information_Objects;

namespace Search.Logic.Interfaces
{
    public interface IRepository
    {
        #region Minimized Client Data Getters
        /// <summary>
        /// Gets a list of minimized client data for speed and parsing and data purposes
        /// </summary>
        /// <returns></returns>
        Task<List<StrippedClient>> GetAllStrippedClientData();
        #endregion

        #region Utility
        /// <summary>
        /// Search a list of clients and returns a list based on a model of queries
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        Task<List<StrippedClient>> SearchClientByQuery(SearchQueries searchQuery);
        #endregion
    }
}