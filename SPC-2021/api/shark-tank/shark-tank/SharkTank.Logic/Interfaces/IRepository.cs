using SharkTank.Logic.Objects.Base_Objects.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTank.Logic.Interfaces
{
    public interface IRepository
    {
        #region Category
        /// <summary>
        /// Gets a list of all the categories
        /// </summary>
        /// <returns></returns>
        Task<List<Category>> GetAllCategories();
        #endregion

        #region Yule Log
        /// <summary>
        /// Creates a new log entry with a logic YuleLog object
        /// </summary>
        /// <param name="newLog"></param>
        /// <returns></returns>
        Task CreateNewLogEntry(YuleLog newLog);
        /// <summary>
        /// Gets a list of all current log entries
        /// </summary>
        /// <returns></returns>
        Task<List<YuleLog>> GetAllLogEntries();
        /// <summary>
        /// Gets a specific log by ID
        /// </summary>
        /// <param name="logID"></param>
        /// <returns></returns>
        Task<YuleLog> GetLogByID(Guid logID);
        /// <summary>
        /// Gets a list of logs by a category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        Task<List<YuleLog>> GetLogsByCategoryID(Guid categoryID);
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
