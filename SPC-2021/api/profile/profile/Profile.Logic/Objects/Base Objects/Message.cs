using Profile.Logic.Objects.Information_Objects;
using System;

namespace Profile.Logic.Objects
{
    public class Message
    {
        public Guid chatMessageID { get; set; }
        public ClientChatMeta senderClient { get; set; }
        public ClientChatMeta recieverClient { get; set; }
        public Guid? clientRelationXrefID { get; set; }
        public string messageContent { get; set; }
        public DateTime dateTimeSent { get; set; }
        public bool isMessageRead { get; set; }
        public bool fromAdmin { get; set; }
        /// <summary>
        /// This is a boolean the API returns to tell the front end that the message should be blue for the subject. Use case being if an admin messages themselves. 
        /// </summary>
        public bool subjectMessage { get; set; }
    }
}
