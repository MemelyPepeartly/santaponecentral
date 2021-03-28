using Event.Data.Entities;
using Event.Logic.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Event.Data.Repository
{
    public static class Mapper
    {
        #region Event
        public static Logic.Objects.Event MapEvent(EventType contextEventType)
        {
            Logic.Objects.Event logicEvent = new Logic.Objects.Event()
            {
                eventTypeID = contextEventType.EventTypeId,
                eventDescription = contextEventType.EventDescription,
                active = contextEventType.IsActive,
                removable = contextEventType.ClientRelationXrefs.Count == 0 && contextEventType.Surveys.Count == 0,
                immutable = contextEventType.EventDescription == Constants.CARD_EXCHANGE_EVENT || contextEventType.EventDescription == Constants.GIFT_EXCHANGE_EVENT
            };
            return logicEvent;
        }
        public static EventType MapEvent(Logic.Objects.Event logicEvent)
        {
            EventType contextEvent = new EventType()
            {
                EventTypeId = logicEvent.eventTypeID,
                EventDescription = logicEvent.eventDescription,
                IsActive = logicEvent.active
            };
            return contextEvent;
        }
        #endregion
    }
}
