using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Message
    {
        public Guid chatMessageID { get; set; }
        public MessageClientMeta senderClient { get; set; }
        public MessageClientMeta recieverClient { get; set; }
        public Guid? clientRelationXrefID { get; set; }
        public string messageContent { get; set; }
        public DateTime dateTimeSent { get; set; }
        public bool isMessageRead { get; set; }

    }
}
