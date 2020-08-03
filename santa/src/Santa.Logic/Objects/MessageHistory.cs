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
        /// <summary>
        /// Meta for the client who the conversation is with (Not the admin). Effectively the sender client meta information
        /// </summary>
        public ClientMeta conversationClient { get; set; }
        public Event eventType { get; set; }
        /// <summary>
        /// Meta information for the person sending the gift if it is a history with a relationXrefID
        /// </summary>
#warning Possibly depreciated property
        public ClientMeta eventSenderClient { get; set; }
        /// <summary>
        /// Meta information for the person recieving the gift if it is a history with a relationXrefID
        /// </summary>
        public ClientMeta eventRecieverClient { get; set; }

    }
}
