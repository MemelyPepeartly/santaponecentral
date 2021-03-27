using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class ClientRelationXref
    {
        public ClientRelationXref()
        {
            ChatMessage = new HashSet<ChatMessage>();
        }

        public Guid ClientRelationXrefId { get; set; }
        public Guid SenderClientId { get; set; }
        public Guid RecipientClientId { get; set; }
        public Guid EventTypeId { get; set; }
        public Guid AssignmentStatusId { get; set; }

        public virtual AssignmentStatus AssignmentStatus { get; set; }
        public virtual EventType EventType { get; set; }
        public virtual Client RecipientClient { get; set; }
        public virtual Client SenderClient { get; set; }
        public virtual ICollection<ChatMessage> ChatMessage { get; set; }
    }
}
