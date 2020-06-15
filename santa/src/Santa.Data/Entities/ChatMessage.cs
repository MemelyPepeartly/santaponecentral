using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class ChatMessage
    {
        public Guid ChatMessageId { get; set; }
        public Guid? MessageSenderClientId { get; set; }
        public Guid? MessageRecieverClientId { get; set; }
        public Guid? ClientRelationXrefId { get; set; }
        public bool IsMessageRead { get; set; }
        public string MessageContent { get; set; }

        public virtual ClientRelationXref ClientRelationXref { get; set; }
        public virtual Client MessageRecieverClient { get; set; }
        public virtual Client MessageSenderClient { get; set; }
    }
}
