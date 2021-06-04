using Message.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Message.Logic.Objects
{
    /// <summary>
    /// Container for information and messages for a given message history
    /// </summary>
    public class MessageHistory
    {
        /// <summary>
        /// Event that the chat history is for
        /// </summary>
        public Event eventType { get; set; }
        /// <summary>
        /// List of all the messages for this particular event and agent. Contains all the message to and from admins, and to and from the target agent
        /// </summary>
        public List<ChatMessage> chatMessages { get; set; }
        /// <summary>
        /// Holder of the conversation. This is the agent that was assigned anons for the event
        /// </summary>
        public ClientChatMeta agent { get; set; }
        /// <summary>
        /// Boolean to determine if a history is a general history or not. Being a general history implies that no event type exists
        /// </summary>
        public bool isGeneralHistory { get; set; }

    }
}
