using SharkTank.Logic.Objects;
using SharkTank.Logic.Objects.Base_Objects.Logging;
using SharkTank.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTank.Logic.Interfaces
{
    public interface IRepository
    {
        #region Client
        /// <summary>
        /// Gets a basic client type by ID
        /// </summary>
        /// <returns></returns>
        Task<BaseClient> GetBasicClientInformationByID(Guid clientID);
        /// <summary>
        /// Gets a basic client type by email
        /// </summary>
        /// <returns></returns>
        Task<BaseClient> GetBasicClientInformationByEmail(string clientEmail);
        #endregion

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

        #region Informational Containers
        /// <summary>
        /// Gets a list of assignments for a client by their ID with additional information that a RelationshipMeta provides
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        Task<List<RelationshipMeta>> getClientAssignmentsInfoByIDAsync(Guid clientID);
        /// <summary>
        /// Gets a specific relationship for a client by xrefID
        /// </summary>
        /// <param name="xrefID"></param>
        /// <returns></returns>
        Task<RelationshipMeta> getAssignmentRelationshipMetaByIDAsync(Guid xrefID);
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