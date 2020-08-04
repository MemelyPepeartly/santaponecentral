using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Message
    {
        public Guid chatMessageID { get; set; }
        public ClientMeta senderClient { get; set; }
        public ClientMeta recieverClient { get; set; }
        public Guid? clientRelationXrefID { get; set; }
        public string messageContent { get; set; }
        public DateTime dateTimeSent { get; set; }
        public bool isMessageRead { get; set; }
        /// <summary>
        /// This is a boolean the API returns to tell the front end that the message should be blue for the subject. Use case being if an admin messages themselves. 
        /// </summary>
        public bool subjectMessage { get; set; }
    }
}
