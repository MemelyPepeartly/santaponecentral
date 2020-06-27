using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Santa.Logic.Objects
{
    public class MessageHistory
    {
        public List<Message> history { get; set; }
        public Guid? relationXrefID { get; set; }
        public ClientMeta conversationClient { get; set; }
        public Event eventType { get; set; }
        /// <summary>
        /// Meta information for the person sending the gift if it is a history with a relationXrefID
        /// </summary>
        public ClientMeta eventSenderClient { get; set; }
        /// <summary>
        /// Meta information for the person recieving the gift if it is a history with a relationXrefID
        /// </summary>
        public ClientMeta eventRecieverClient { get; set; }

    }
}
