using Santa.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Santa.Logic.Objects
{
    public class MessageHistory
    {
        public Guid? relationXrefID { get; set; }
        public Event eventType { get; set; }
        /// <summary>
        /// Meta for the client that is involved with the conversation. This is primarily for the front ent to determine information about general histories
        /// where no assingmentSender or reciever is present
        /// </summary>
        public ClientChatMeta conversationClient { get; set; }
        /// <summary>
        /// Status of the assignment this history relates to
        /// </summary>
        public AssignmentStatus assignmentStatus { get; set; }
        /// <summary>
        /// Meta of the client that the conversation is about. This is the client that was assigned if the object has a relationXrefID
        /// </summary>
        public ClientChatMeta assignmentRecieverClient { get; set; }
        /// <summary>
        /// Meta of the client that is sending the gift if a relationship is present
        /// </summary>
        public ClientChatMeta assignmentSenderClient { get; set; }
        /// <summary>
        /// Subject client is the one who is requesting to see the messages. Will always be on the blue side
        /// </summary>
        public ClientChatMeta subjectClient { get; set; }
        /// <summary>
        /// List of the subject client's messages
        /// </summary>
        public List<Message> subjectMessages { get; set; }
        /// <summary>
        /// Reciever messages on the grey side. There is no client meta because more than one admin can be the people sending a message
        /// </summary>
        public List<Message> recieverMessages { get; set; }
        /// <summary>
        /// Count of all unread messages on the reciever messages
        /// </summary>
        public int unreadCount { get; set; }

    }
}
