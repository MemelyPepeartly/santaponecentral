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
        /* Subject ID's are needed to determine who was the client that made the call. For example, if a person with a profile wants their messages,
           they will call the endpoint with themselves as the subject
        */
        /// <summary>
        /// Gets all chat histories. This includes general and assignment correspondence
        /// </summary>
        /// <param name="subjectClient"></param>
        /// <returns></returns>
        Task<List<MessageHistory>> GetAllChatHistories(BaseClient subjectClient);
        /// <summary>
        /// Gets a list of a client's assignment chats by ID. This is namely used for profiles, and uses a base client object that also acts as the subject of a message history
        /// </summary>
        /// <param name="subjectClient"></param>
        /// <returns></returns>
        Task<List<MessageHistory>> GetAllAssignmentChatsByClientID(BaseClient subjectClient);
        Task<MessageHistory> GetChatHistoryByXrefIDAndSubjectIDAsync(Guid clientRelationXrefID, BaseClient subjectClient);
        Task<MessageHistory> GetGeneralChatHistoryBySubjectIDAsync(Client conversationClient, BaseClient subjectClient);
        Task<List<MessageHistory>> GetUnloadedProfileChatHistoriesAsync(Guid profileOwnerClientID);
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
