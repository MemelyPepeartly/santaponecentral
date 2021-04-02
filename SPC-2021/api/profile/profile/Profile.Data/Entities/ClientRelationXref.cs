using System;
using System.Collections.Generic;

#nullable disable

namespace Profile.Data.Entities
{
    public partial class ClientRelationXref
    {
        public ClientRelationXref()
        {
            ChatMessages = new HashSet<ChatMessage>();
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
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
