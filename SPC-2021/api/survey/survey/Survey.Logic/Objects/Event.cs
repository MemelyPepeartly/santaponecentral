using System;

namespace Survey.Logic.Objects
{
    public class Event
    {
        public Guid eventTypeID { get; set; }
        public string eventDescription { get; set; }
        public bool active { get; set; }
        public bool removable { get; set; }
        public bool immutable { get; set; }
    }
}
