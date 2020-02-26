using System;

namespace Santa.Logic.Objects
{
    public class Event
    {
        public Guid eventTypeID { get; set; }
        public string eventDescription { get; set; }
        public bool active { get; set; }
    }
}
