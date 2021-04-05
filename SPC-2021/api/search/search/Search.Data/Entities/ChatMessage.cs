using System;
using System.Collections.Generic;

#nullable disable

namespace Search.Data.Entities
{
    public partial class ChatMessage
    {
        public Guid ChatMessageId { get; set; }
        public Guid? MessageSenderClientId { get; set; }
        public Guid? MessageReceiverClientId { get; set; }
        public Guid? ClientRelationXrefId { get; set; }
        public string MessageContent { get; set; }
        public DateTime DateTimeSent { get; set; }
        public bool IsMessageRead { get; set; }
        public bool FromAdmin { get; set; }

        public virtual ClientRelationXref ClientRelationXref { get; set; }
        public virtual Client MessageReceiverClient { get; set; }
        public virtual Client MessageSenderClient { get; set; }
    }
}
