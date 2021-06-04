using Message.Logic.Objects.Information_Objects;
using ChatMessage = Message.Logic.Objects.ChatMessage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Message.Logic.Objects;
using Message.Data.Repository;
using Santa.Logic.Constants;
using Message.Logic.Interfaces;

namespace Message.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly Message.Data.Entities.SantaPoneCentralDatabaseContext santaContext;

        public Repository(Message.Data.Entities.SantaPoneCentralDatabaseContext _context)
        {
            santaContext = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        #region Client
        public async Task<BaseClient> GetBasicClientInformationByID(Guid clientID)
        {
            BaseClient logicBaseClient = await santaContext.Clients
                .Select(client => new BaseClient()
                {
                    clientID = client.ClientId,
                    clientName = client.ClientName,
                    nickname = client.Nickname,
                    email = client.Email,
                    isAdmin = client.IsAdmin,
                    hasAccount = client.HasAccount,

                }).AsNoTracking().FirstOrDefaultAsync(c => c.clientID == clientID);
            return logicBaseClient;
        }

        public async Task<BaseClient> GetBasicClientInformationByEmail(string clientEmail)
        {
            BaseClient logicBaseClient = await santaContext.Clients
            .Select(client => new BaseClient()
            {
                clientID = client.ClientId,
                clientName = client.ClientName,
                nickname = client.Nickname,
                email = client.Email,
                isAdmin = client.IsAdmin,
                hasAccount = client.HasAccount,

            }).FirstOrDefaultAsync(c => c.email == clientEmail);
            return logicBaseClient;
        }
        #endregion

        #region Message
        public async Task CreateMessage(ChatMessage newMessage)
        {
            Message.Data.Entities.ChatMessage contextMessage = Mapper.MapMessage(newMessage);
            await santaContext.ChatMessages.AddAsync(contextMessage);
        }
        public async Task<List<ChatMessage>> GetAllMessages()
        {
            List<ChatMessage> logicMessageList = (await santaContext.ChatMessages
                .Include(s => s.MessageSenderClient)
                .Include(r => r.MessageReceiverClient)
                .Include(x => x.ClientRelationXref)
                .ToListAsync())
                .Select(Mapper.MapMessage).ToList();

            return logicMessageList;
        }
        public async Task<ChatMessage> GetMessageByIDAsync(Guid chatMessageID)
        {
            ChatMessage logicMessage = await santaContext.ChatMessages
                .Select(message => new ChatMessage()
                {
                    chatMessageID = message.ChatMessageId,
                    clientRelationXrefID = message.ClientRelationXrefId != null ? message.ClientRelationXrefId : null,
                    recieverClient = new ClientChatMeta()
                    {
                        clientId = message.MessageReceiverClientId != null ? message.MessageReceiverClientId : null,
                        clientNickname = message.MessageReceiverClientId != null ? message.MessageReceiverClient.Nickname : String.Empty
                    },
                    senderClient = new ClientChatMeta()
                    {
                        clientId = message.MessageSenderClientId != null ? message.MessageSenderClientId : null,
                        clientNickname = message.MessageSenderClientId != null ? message.MessageSenderClient.Nickname : String.Empty
                    },
                    messageContent = message.MessageContent,
                    dateTimeSent = message.DateTimeSent,
                    isMessageRead = message.IsMessageRead,
                    fromAdmin = message.FromAdmin
                }).FirstOrDefaultAsync(m => m.chatMessageID == chatMessageID);
            return logicMessage;
        }
        public async Task UpdateMessageByIDAsync(ChatMessage targetMessage)
        {
            Message.Data.Entities.ChatMessage contextMessage = await santaContext.ChatMessages.FirstOrDefaultAsync(m => m.ChatMessageId == targetMessage.chatMessageID);

            contextMessage.IsMessageRead = targetMessage.isMessageRead;

            santaContext.ChatMessages.Update(contextMessage);
        }
        public async Task DeleteMessageByID(Guid chatMessageID)
        {
            Message.Data.Entities.ChatMessage contextMessage = await santaContext.ChatMessages.FirstOrDefaultAsync(m => m.ChatMessageId == chatMessageID);
            santaContext.ChatMessages.Remove(contextMessage);
        }

        #region Message Histories
        public async Task<List<MessageHistory>> GetAllChatHistories(BaseClient subjectClient)
        {
            throw new NotImplementedException();
        //    List<Message.Data.Entities.Client> contextClients = await santaContext.Clients.Include(c => c.ClientStatus).Where(c => c.ClientStatus.StatusDescription != Constants.AWAITING_STATUS && c.ClientStatus.StatusDescription != Constants.DENIED_STATUS).ToListAsync();

        //    /* Assignment history query */
        //    /*
        //     * Xref is not null
        //     * Event type is the type of event attatched to the xref
        //     * Assignment status is attached to xref
        //     * Subject client is the meta passed in the method
        //     * Conversation client is the sender client (The agent)
        //     * Assignment sender is the sender client (The agent)
        //     * Assignment reciever client is the reciever client (The actual assignment client)
        //     * 
        //     * Subject and reciever messages are new lists for loading times since histories are lazy loaded in the app
        //     * 
        //    */
        //    List<MessageHistory> listLogicAssignmentMessageHistory = await santaContext.ClientRelationXrefs
        //        .Select(xref => new MessageHistory()
        //        {
        //            relationXrefID = xref.ClientRelationXrefId,
        //            eventType = new Event()
        //            {
        //                eventTypeID = xref.EventType.EventTypeId,
        //                eventDescription = xref.EventType.EventDescription,
        //                active = xref.EventType.IsActive,
        //                removable = xref.EventType.ClientRelationXrefs.Count == 0 && xref.EventType.Surveys.Count == 0,
        //                immutable = xref.EventType.EventDescription == Constants.CARD_EXCHANGE_EVENT || xref.EventType.EventDescription == Constants.GIFT_EXCHANGE_EVENT
        //            },
        //            assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),

        //            subjectClient = Mapper.MapClientChatMeta(subjectClient),
        //            conversationClient = Mapper.MapClientChatMeta(xref.SenderClient),
        //            assignmentRecieverClient = Mapper.MapClientChatMeta(xref.RecipientClient),
        //            assignmentSenderClient = Mapper.MapClientChatMeta(xref.SenderClient),

        //            subjectMessages = new List<ChatMessage>(),
        //            recieverMessages = new List<ChatMessage>(),

        //            unreadCount = xref.ChatMessages
        //                .Where(m => !m.FromAdmin && m.IsMessageRead == false)
        //                .Count()
        //        }).ToListAsync();

        //    /* General history query */
        //    /*
        //     * All clients where the status of the client is approved or completed (Awaiting and denied have no chat history)
        //     * 
        //     * Xref is null
        //     * Event type does not exist
        //     * Assignment status does not exist
        //     * Subject client is the meta passed in the method
        //     * Conversation client is the client object in the context
        //     * Assingment sender/reciever clients do not exist
        //     * 
        //     * Subject and reciever messages are new lists for loading times since histories are lazy loaded in the app
        //    */
        //    List<MessageHistory> logicGeneralChatHistories = await santaContext.Clients.Where(c => c.ClientStatus.StatusDescription == Constants.APPROVED_STATUS || c.ClientStatus.StatusDescription == Constants.COMPLETED_STATUS)
        //        .Select(client => new MessageHistory()
        //        {
        //            relationXrefID = null,
        //            eventType = new Event(),
        //            assignmentStatus = new AssignmentStatus(),

        //            subjectClient = Mapper.MapClientChatMeta(subjectClient),
        //            conversationClient = Mapper.MapClientChatMeta(client),
        //            assignmentRecieverClient = new ClientChatMeta(),
        //            assignmentSenderClient = new ClientChatMeta(),

        //            subjectMessages = new List<ChatMessage>(),
        //            recieverMessages = new List<ChatMessage>(),

        //            unreadCount = client.ChatMessageMessageSenderClients
        //            .Where(m => m.ClientRelationXrefId == null && !m.MessageSenderClient.IsAdmin && m.IsMessageRead == false)
        //            .Count()
        //        }).ToListAsync();

        //    List<MessageHistory> totalHistories = listLogicAssignmentMessageHistory.Concat(logicGeneralChatHistories).ToList();


        //    return totalHistories.OrderByDescending(h => h.eventType.eventDescription).ThenBy(h => h.conversationClient.clientNickname).ToList();
        }

        public async Task<List<MessageHistory>> GetProfileChatHistories(Guid profileOwnerClientID)
        {
            throw new NotImplementedException();
            /*
            Client logicSubject = await GetStaticClientObjectByID(profileOwnerClientID);
            List<MessageHistory> listLogicMessageHistory = await santaContext.ClientRelationXrefs.Where(r => r.SenderClientId == profileOwnerClientID)
                .Select(xref => new MessageHistory()
                {
                    relationXrefID = xref.ClientRelationXrefId,
                    eventType = Mapper.MapEvent(xref.EventType),
                    assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),

                    subjectClient = Mapper.MapClientChatMeta(logicSubject),
                    conversationClient = Mapper.MapClientChatMeta(xref.SenderClient),
                    assignmentRecieverClient = Mapper.MapClientChatMeta(xref.RecipientClient),
                    assignmentSenderClient = Mapper.MapClientChatMeta(xref.SenderClient),

                    subjectMessages = new List<ChatMessage>(),
                    recieverMessages = new List<ChatMessage>(),

                    unreadCount = xref.ChatMessages.Where(m => m.FromAdmin && m.IsMessageRead == false).Count()
                }).ToListAsync();


            return listLogicMessageHistory;
            */
        }
        public async Task<MessageHistory> GetSpecificHistoryByClientIDAndEventID(Guid conversationAgentID, Guid eventTypeID)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region Utility
        public async Task SaveAsync()
        {
            await santaContext.SaveChangesAsync();
        }

        public async Task<Client> GetStaticClientObjectByID(Guid clientID)
        {
            return Mapper.MapStaticClient((await santaContext.Clients.AsNoTracking().FirstAsync(c => c.ClientId == clientID)));
        }
        #endregion
    }
}
