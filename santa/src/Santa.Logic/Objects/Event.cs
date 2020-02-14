using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Event
    {
        public Guid eventTypeID { get; set; }
        public string eventDescription { get; set; }
        public bool active { get; set; }
    }
}
