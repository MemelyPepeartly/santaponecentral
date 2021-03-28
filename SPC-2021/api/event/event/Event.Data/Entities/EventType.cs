using System;
using System.Collections.Generic;

#nullable disable

namespace Event.Data.Entities
{
    public partial class EventType
    {
        public EventType()
        {
            ClientRelationXrefs = new HashSet<ClientRelationXref>();
            Surveys = new HashSet<Survey>();
        }

        public Guid EventTypeId { get; set; }
        public string EventDescription { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<ClientRelationXref> ClientRelationXrefs { get; set; }
        public virtual ICollection<Survey> Surveys { get; set; }
    }
}
