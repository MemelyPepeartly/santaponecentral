using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class MessageHistory
    {
        public List<Message> history { get; set; }
        public Guid? relationXrefID { get; set; }
        public Event eventType { get; set; }
        public MessageClientMeta eventSenderClient { get; set; }
        public MessageClientMeta eventRecieverClient { get; set; }
    }
}
