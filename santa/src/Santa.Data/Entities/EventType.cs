using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class EventType
    {
        public EventType()
        {
            ClientRelationXref = new HashSet<ClientRelationXref>();
            Survey = new HashSet<Survey>();
        }

        public int EventTypeId { get; set; }
        public string EventDescription { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<ClientRelationXref> ClientRelationXref { get; set; }
        public virtual ICollection<Survey> Survey { get; set; }
    }
}
