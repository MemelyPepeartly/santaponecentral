using Message.Logic.Objects;
using Message.Logic.Objects.Information_Objects;
using Santa.Logic.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Data.Repository
{
    public static class Mapper
    {
        #region Client
        public static Client MapStaticClient(Entities.Client contextClient)
        {
            Client logicClient = new Client()
            {
                clientID = contextClient.ClientId,
                email = contextClient.Email,
                clientName = contextClient.ClientName,
                nickname = contextClient.Nickname,
                hasAccount = contextClient.HasAccount,
                isAdmin = contextClient.IsAdmin,
                address = new Address
                {
                    addressLineOne = contextClient.AddressLine1,
                    addressLineTwo = contextClient.AddressLine2,
                    city = contextClient.City,
                    country = contextClient.Country,
                    state = contextClient.State,
                    postalCode = contextClient.PostalCode
                }
            };
            return logicClient;
        }
        #endregion

        #region Meta
        public static ClientChatMeta MapClientChatMeta(Entities.Client contextClient)
        {
            ClientChatMeta logicMeta = new ClientChatMeta()
            {
                clientId = contextClient.ClientId,
                clientNickname = contextClient.Nickname,
                hasAccount = contextClient.HasAccount,
                isAdmin = contextClient.IsAdmin
            };

            return logicMeta;
        }
        public static ClientChatMeta MapClientChatMeta(BaseClient baseLogicClient)
        {
            ClientChatMeta logicMeta = new ClientChatMeta()
            {
                clientId = baseLogicClient.clientID,
                clientNickname = baseLogicClient.nickname,
                hasAccount = baseLogicClient.hasAccount,
                isAdmin = baseLogicClient.isAdmin
            };

            return logicMeta;
        }
        public static ClientChatMeta MapClientChatMeta(Logic.Objects.Client logicClient)
        {
            ClientChatMeta logicMeta = new ClientChatMeta()
            {
                clientId = logicClient.clientID,
                clientNickname = logicClient.nickname,
                hasAccount = logicClient.hasAccount,
                isAdmin = logicClient.isAdmin
            };

            return logicMeta;
        }
        #endregion

        #region Message History
        /// <summary>
        /// Maps the information of a message history with the contents of a relationship, and the viewer subject client information
        /// </summary>
        /// <param name="contextRelationshipXref"></param>
        /// <param name="logicSubjectClient"></param>
        /// <returns></returns>
        public static Logic.Objects.MessageHistory MapHistoryInformation(Entities.ClientRelationXref contextRelationshipXref, BaseClient logicSubjectClient, bool unloaded)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Maps history information for general chats, which do not have a relationshipXrefID, EventType, or AssignmentClient using a list of contextChatMessages relating to the client's general conversation
        /// </summary>
        /// <param name="contextConversationClient"></param>
        /// <param name="contextChatMessages"></param>
        /// <param name="logicSubjectClient"></param>
        /// <returns></returns>
        public static MessageHistory MapHistoryInformation(Entities.Client contextConversationClient, List<Entities.ChatMessage> contextChatMessages, BaseClient logicSubjectClient)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps history information for general chats, which do not have a relationshipXrefID, EventType, or AssignmentClient using a list of contextChatMessages relating to the client's general conversation. Uses a logic client object
        /// rather than a context client object
        /// </summary>
        /// <param name="logicConversationClient"></param>
        /// <param name="contextChatMessages"></param>
        /// <param name="logicSubjectClient"></param>
        /// <returns></returns>
        public static Logic.Objects.MessageHistory MapHistoryInformation(Client logicConversationClient, List<Entities.ChatMessage> contextChatMessages, BaseClient logicSubjectClient)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Event

        public static Logic.Objects.Event MapEvent(Entities.EventType contextEventType)
        {
            Logic.Objects.Event logicEvent = new Logic.Objects.Event()
            {
                eventTypeID = contextEventType.EventTypeId,
                eventDescription = contextEventType.EventDescription,
                active = contextEventType.IsActive,
                removable = contextEventType.ClientRelationXrefs.Count == 0 && contextEventType.Surveys.Count == 0,
                immutable = contextEventType.EventDescription == Constants.CARD_EXCHANGE_EVENT || contextEventType.EventDescription == Constants.GIFT_EXCHANGE_EVENT
            };
            return logicEvent;
        }
        public static Entities.EventType MapEvent(Event logicEvent)
        {
            Entities.EventType contextEvent = new Entities.EventType()
            {
                EventTypeId = logicEvent.eventTypeID,
                EventDescription = logicEvent.eventDescription,
                IsActive = logicEvent.active
            };
            return contextEvent;
        }
        #endregion

        #region Message

        public static ChatMessage MapMessage(Entities.ChatMessage contextMessage)
        {
            ChatMessage logicMessage = new ChatMessage()
            {
                chatMessageID = contextMessage.ChatMessageId,
                clientRelationXrefID = contextMessage.ClientRelationXrefId != null ? contextMessage.ClientRelationXrefId : null,
                recieverClient = new ClientChatMeta()
                {
                    clientId = contextMessage.MessageReceiverClientId != null ? contextMessage.MessageReceiverClientId : null,
                    clientNickname = contextMessage.MessageReceiverClientId != null ? contextMessage.MessageReceiverClient.Nickname : String.Empty
                },
                senderClient = new ClientChatMeta()
                {
                    clientId = contextMessage.MessageSenderClientId != null ? contextMessage.MessageSenderClientId : null,
                    clientNickname = contextMessage.MessageSenderClientId != null ? contextMessage.MessageSenderClient.Nickname : String.Empty
                },
                messageContent = contextMessage.MessageContent,
                dateTimeSent = contextMessage.DateTimeSent,
                isMessageRead = contextMessage.IsMessageRead,
                fromAdmin = contextMessage.FromAdmin
            };
            return logicMessage;
        }

        public static Entities.ChatMessage MapMessage(ChatMessage logicMessage)
        {
            Entities.ChatMessage contextMessage = new Entities.ChatMessage()
            {
                ChatMessageId = logicMessage.chatMessageID,
                ClientRelationXrefId = logicMessage.clientRelationXrefID,
                MessageReceiverClientId = logicMessage.recieverClient.clientId,
                MessageSenderClientId = logicMessage.senderClient.clientId,
                MessageContent = logicMessage.messageContent,
                DateTimeSent = logicMessage.dateTimeSent,
                IsMessageRead = logicMessage.isMessageRead,
                FromAdmin = logicMessage.fromAdmin
            };
            return contextMessage;
        }

        #endregion

        #region Assignment Status
        public static Entities.AssignmentStatus MapAssignmentStatus(Logic.Objects.AssignmentStatus logicAssignmentStatus)
        {
            Entities.AssignmentStatus contextAssignmentStatus = new Entities.AssignmentStatus()
            {
                AssignmentStatusId = logicAssignmentStatus.assignmentStatusID,
                AssignmentStatusName = logicAssignmentStatus.assignmentStatusName,
                AssignmentStatusDescription = logicAssignmentStatus.assignmentStatusDescription
            };

            return contextAssignmentStatus;
        }

        public static Logic.Objects.AssignmentStatus MapAssignmentStatus(Entities.AssignmentStatus contextAssignmentStatus)
        {
            Logic.Objects.AssignmentStatus logicAssignmentStatus = new Logic.Objects.AssignmentStatus()
            {
                assignmentStatusID = contextAssignmentStatus.AssignmentStatusId,
                assignmentStatusName = contextAssignmentStatus.AssignmentStatusName,
                assignmentStatusDescription = contextAssignmentStatus.AssignmentStatusDescription
            };

            return logicAssignmentStatus;
        }
        #endregion
    }
}
