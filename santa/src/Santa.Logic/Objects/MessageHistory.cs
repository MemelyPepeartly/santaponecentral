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
        /// <summary>
        /// Meta information for the person sending the gift if it is a history with a relationXrefID
        /// </summary>
        public MessageClientMeta eventSenderClient { get; set; }
        /// <summary>
        /// Meta information for the person recieving the gift if it is a history with a relationXrefID
        /// </summary>
        public MessageClientMeta eventRecieverClient { get; set; }
    }
}
