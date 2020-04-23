using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class ClientRelationXref
    {
        public int ClientRelationXrefId { get; set; }
        public Guid SenderClientId { get; set; }
        public Guid RecipientClientId { get; set; }
        public Guid EventTypeId { get; set; }

        public virtual EventType EventType { get; set; }
        public virtual Client RecipientClient { get; set; }
        public virtual Client SenderClient { get; set; }
    }
}
