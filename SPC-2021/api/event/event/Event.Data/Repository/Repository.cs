using Event.Data.Entities;
using Event.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly SantaPoneCentralDatabaseContext santaContext;

        public Repository(SantaPoneCentralDatabaseContext _context)
        {
            santaContext = _context ?? throw new ArgumentNullException(nameof(_context));
        }
        
        #region Event
        public async Task CreateEventAsync(Logic.Objects.Event newEvent)
        {
            Data.Entities.EventType contextEvent = Mapper.MapEvent(newEvent);
            await santaContext.EventTypes.AddAsync(contextEvent);
        }
        public async Task DeleteEventByIDAsync(Guid eventID)
        {
            Data.Entities.EventType contextEvent = await santaContext.EventTypes.FirstOrDefaultAsync(e => e.EventTypeId == eventID);
            santaContext.EventTypes.Remove(contextEvent);
        }
        public async Task<List<Logic.Objects.Event>> GetAllEvents()
        {
            List<Logic.Objects.Event> eventList = (await santaContext.EventTypes
                .Include(e => e.ClientRelationXrefs)
                .Include(e => e.Surveys)
                .ToListAsync())
                .Select(Mapper.MapEvent)
                .ToList();
            return eventList;
        }
        public async Task<Logic.Objects.Event> GetEventByIDAsync(Guid eventID)
        {
            Logic.Objects.Event logicEvent = Mapper.MapEvent(await santaContext.EventTypes
                .Include(e => e.ClientRelationXrefs)
                .Include(e => e.Surveys)
                .FirstOrDefaultAsync(e => e.EventTypeId == eventID));
            return logicEvent;
        }
        public async Task<Logic.Objects.Event> GetEventByNameAsync(string eventName)
        {
            Logic.Objects.Event logicEvent = Mapper.MapEvent(await santaContext.EventTypes
                .Include(e => e.Surveys)
                .Include(e => e.ClientRelationXrefs)
                .FirstOrDefaultAsync(e => e.EventDescription == eventName));
            return logicEvent;
        }
        public async Task UpdateEventByIDAsync(Logic.Objects.Event targetEvent)
        {
            Data.Entities.EventType oldContextEvent = await santaContext.EventTypes.FirstOrDefaultAsync(e => e.EventTypeId == targetEvent.eventTypeID);

            oldContextEvent.EventDescription = targetEvent.eventDescription;
            oldContextEvent.IsActive = targetEvent.active;

            santaContext.Update(oldContextEvent);
        }
        #endregion

        #region Utility
        public async Task SaveAsync()
        {
            await santaContext.SaveChangesAsync();
        }
        #endregion
    }
}
