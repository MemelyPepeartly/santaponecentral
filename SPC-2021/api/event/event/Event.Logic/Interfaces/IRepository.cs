using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Event.Logic.Interfaces
{
    public interface IRepository
    {
        #region Event
        Task CreateEventAsync(Objects.Event newEvent);
        Task<List<Logic.Objects.Event>> GetAllEvents();
        Task<Objects.Event> GetEventByIDAsync(Guid eventID);
        Task<Objects.Event> GetEventByNameAsync(string eventName);
        Task UpdateEventByIDAsync(Objects.Event targetEvent);
        Task DeleteEventByIDAsync(Guid logicEvent);
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
