using Message.Logic.Objects;
using Message.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Logic.Interfaces
{
    public interface IRepository
    {
        #region Client
        /// <summary>
        /// Gets a basic client type by ID
        /// </summary>
        /// <returns></returns>
        Task<BaseClient> GetBasicClientInformationByID(Guid clientID);
        /// <summary>
        /// Gets a basic client type by email
        /// </summary>
        /// <returns></returns>
        Task<BaseClient> GetBasicClientInformationByEmail(string clientEmail);
        #endregion

        #region Message
        Task CreateMessage(ChatMessage newMessage);
        Task<List<ChatMessage>> GetAllMessages();
        Task<ChatMessage> GetMessageByIDAsync(Guid chatMessageID);
        Task UpdateMessageByIDAsync(ChatMessage targetMessage);
        Task DeleteMessageByID(Guid chatMessageID);

        #region Message Histories
        /// <summary>
        /// Gets a list of all chat histories with filled chat messages
        /// </summary>
        /// <param name="subjectClient"></param>
        /// <returns></returns>
        Task<List<MessageHistory>> GetAllChatHistories(BaseClient subjectClient);
        /// <summary>
        /// Gets a list of all chat histories for a given client's profile with filled messages
        /// </summary>
        /// <param name="profileOwnerClientID"></param>
        /// <returns></returns>
        Task<List<MessageHistory>> GetProfileChatHistories(Guid profileOwnerClientID);
        /// <summary>
        /// Returns a specific chat history by the agent who holds the conversation, and the event it is for
        /// </summary>
        /// <param name="conversationAgentID"></param>
        /// <param name="eventTypeID"></param>
        /// <returns></returns>
        Task<MessageHistory> GetSpecificHistoryByClientIDAndEventID(Guid conversationAgentID, Guid eventTypeID);
        #endregion

        #endregion

        #region Utility
        /// <summary>
        /// Saves changes of any CRUD operations in the queue
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
        /// <summary>
        /// Gets a very minimized version of the logic client object by ID. This is for quickly getting info such as the address or email.
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        Task<Client> GetStaticClientObjectByID(Guid clientID);
        #endregion
    }
}
