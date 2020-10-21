using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using Santa.Data.Entities;
using Santa.Logic.Constants;

namespace Santa.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly SantaPoneCentralDatabaseContext santaContext;

        public Repository(SantaPoneCentralDatabaseContext _context)
        {
            santaContext = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        #region Client
        public async Task CreateClient(Logic.Objects.Client newClient)
        {
            Entities.Client contextClient = Mapper.MapClient(newClient);
            await santaContext.Client.AddAsync(contextClient);
        }
        public async Task CreateClientRelationByID(Guid senderClientID, Guid recipientClientID, Guid eventTypeID, Guid assignmentStatusID)
        {
            Data.Entities.ClientRelationXref contexRelation = new ClientRelationXref()
            {
                ClientRelationXrefId = Guid.NewGuid(),
                SenderClientId = senderClientID,
                RecipientClientId = recipientClientID,
                EventTypeId = eventTypeID,
                // Assignment status should be set to its default value as well
                AssignmentStatusId = assignmentStatusID
            };
            await santaContext.ClientRelationXref.AddAsync(contexRelation);
        }
        public async Task<List<Logic.Objects.Client>> GetAllClients()
        {
            List<Logic.Objects.Client> clientList = (await santaContext.Client
                /* Surveys and responses */
                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.SurveyQuestion.SurveyQuestionOptionXref)
                        .ThenInclude(sqox => sqox.SurveyOption)
                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.SurveyQuestion.SurveyQuestionXref)

                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.Survey.EventType)

                /* Sender/Assignment info and Tags */
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(crxsc => crxsc.SenderClient.ClientTagXref)
                        .ThenInclude(txr => txr.Tag)
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(crxrc => crxrc.RecipientClient.ClientTagXref)
                        .ThenInclude(txr => txr.Tag)

                /* Relationship event */
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(crxsc => crxsc.EventType)
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(crxrc => crxrc.EventType)

                /* Chat messages */
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(cm => cm.ChatMessage)
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(cm => cm.ChatMessage)

                /* Assignment statuses */
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(stat => stat.AssignmentStatus)
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(stat => stat.AssignmentStatus)

                /* Tags */
                .Include(c => c.ClientTagXref)
                    .ThenInclude(t => t.Tag)

                /* Client approval status */
                .Include(c => c.ClientStatus)
                .AsNoTracking()
                .ToListAsync())
                .Select(Mapper.MapClient).ToList();

            return clientList;
        }
        public async Task<Logic.Objects.Client> GetClientByIDAsync(Guid clientId)
        {
            Logic.Objects.Client logicClient = Mapper.MapClient(await santaContext.Client
                /* Surveys and responses */
                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.SurveyQuestion.SurveyQuestionOptionXref)
                        .ThenInclude(sqox => sqox.SurveyOption)
                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.SurveyQuestion.SurveyQuestionXref)

                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.Survey.EventType)

                /* Sender/Assignment info and Tags */
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(crxsc => crxsc.SenderClient.ClientTagXref)
                        .ThenInclude(txr => txr.Tag)
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(crxrc => crxrc.RecipientClient.ClientTagXref)
                        .ThenInclude(txr => txr.Tag)

                /* Relationship event */
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(crxsc => crxsc.EventType)
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(crxrc => crxrc.EventType)

                /* Chat messages */
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(cm => cm.ChatMessage)
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(cm => cm.ChatMessage)

                /* Assignment statuses */
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(stat => stat.AssignmentStatus)
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(stat => stat.AssignmentStatus)

                /* Tags */
                .Include(c => c.ClientTagXref)
                    .ThenInclude(t => t.Tag)

                /* Client approval status */
                .Include(c => c.ClientStatus)
                .Where(c => c.ClientId == clientId)
                .AsNoTracking()
                .FirstOrDefaultAsync());

            return logicClient;
        }
        public async Task<Logic.Objects.Client> GetClientByEmailAsync(string clientEmail)
        {
            Logic.Objects.Client logicClient = Mapper.MapClient(await santaContext.Client
                /* Surveys and responses */
                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.SurveyQuestion.SurveyQuestionOptionXref)
                        .ThenInclude(sqox => sqox.SurveyOption)
                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.SurveyQuestion.SurveyQuestionXref)

                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.Survey.EventType)

                /* Sender/Assignment info and Tags */
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(crxsc => crxsc.SenderClient.ClientTagXref)
                        .ThenInclude(txr => txr.Tag)
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(crxrc => crxrc.RecipientClient.ClientTagXref)
                        .ThenInclude(txr => txr.Tag)

                /* Relationship event */
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(crxsc => crxsc.EventType)
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(crxrc => crxrc.EventType)

                /* Chat messages */
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(cm => cm.ChatMessage)
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(cm => cm.ChatMessage)

                /* Assignment statuses */
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(stat => stat.AssignmentStatus)
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(stat => stat.AssignmentStatus)

                /* Tags */
                .Include(c => c.ClientTagXref)
                    .ThenInclude(t => t.Tag)

                /* Client approval status */
                .Include(c => c.ClientStatus)
                .Where(c => c.Email == clientEmail)
                .AsNoTracking()
                .FirstOrDefaultAsync());

            return logicClient;
        }
        public async Task UpdateClientByIDAsync(Logic.Objects.Client targetLogicClient)
        {
            Data.Entities.Client contextOldClient = await santaContext.Client.FirstOrDefaultAsync(c => c.ClientId == targetLogicClient.clientID);

            contextOldClient.ClientName = targetLogicClient.clientName;

            contextOldClient.AddressLine1 = targetLogicClient.address.addressLineOne;
            contextOldClient.AddressLine2 = targetLogicClient.address.addressLineTwo;
            contextOldClient.City = targetLogicClient.address.city;
            contextOldClient.State = targetLogicClient.address.state;
            contextOldClient.Country = targetLogicClient.address.country;
            contextOldClient.PostalCode = targetLogicClient.address.postalCode;

            contextOldClient.ClientStatusId = targetLogicClient.clientStatus.statusID;

            contextOldClient.Email = targetLogicClient.email;
            contextOldClient.Nickname = targetLogicClient.nickname;
            contextOldClient.IsAdmin = targetLogicClient.isAdmin;
            contextOldClient.HasAccount = targetLogicClient.hasAccount;

            santaContext.Client.Update(contextOldClient);
        }
        public async Task UpdateAssignmentProgressStatusByID(Guid assignmentID, Guid newAssignmentStatusID)
        {
            ClientRelationXref contextRelationship = await santaContext.ClientRelationXref.FirstOrDefaultAsync(crxf => crxf.ClientRelationXrefId == assignmentID);

            contextRelationship.AssignmentStatusId = newAssignmentStatusID;

            santaContext.ClientRelationXref.Update(contextRelationship);
        }
        public async Task DeleteClientByIDAsync(Guid clientID)
        {
            Data.Entities.Client contextClient = await santaContext.Client.FirstOrDefaultAsync(c => c.ClientId == clientID);

            santaContext.Client.Remove(contextClient);
        }
        public async Task DeleteRecieverXref(Guid clientID, Guid recipientID, Guid eventID)
        {
            Data.Entities.ClientRelationXref contextRelation = await santaContext.ClientRelationXref.FirstOrDefaultAsync(r => r.SenderClientId == clientID && r.RecipientClientId == recipientID && r.EventTypeId == eventID);
            santaContext.ClientRelationXref.Remove(contextRelation);
        }
        #endregion

        #region Assignment Status
        public async Task CreateAssignmentStatus(Logic.Objects.AssignmentStatus newAssignmentStatus)
        {
            Data.Entities.AssignmentStatus newContextAssignmentStatus = Mapper.MapAssignmentStatus(newAssignmentStatus);
            await santaContext.AssignmentStatus.AddAsync(newContextAssignmentStatus);
        }

        public async Task<List<Logic.Objects.AssignmentStatus>> GetAllAssignmentStatuses()
        {
            List<Logic.Objects.AssignmentStatus> listLogicAssigmentStatus = (await santaContext.AssignmentStatus.ToListAsync()).Select(Mapper.MapAssignmentStatus).ToList();

            return listLogicAssigmentStatus;
        }

        public async Task<Logic.Objects.AssignmentStatus> GetAssignmentStatusByID(Guid assignmentStatusID)
        {
            Logic.Objects.AssignmentStatus logicAssignmentStatus = Mapper.MapAssignmentStatus(await santaContext.AssignmentStatus.FirstOrDefaultAsync(stat => stat.AssignmentStatusId == assignmentStatusID));

            return logicAssignmentStatus;
        }

        public async Task UpdateAssignmentStatus(Logic.Objects.AssignmentStatus targetAssignmentStatus)
        {
            Data.Entities.AssignmentStatus contextAssignmentStatus = await santaContext.AssignmentStatus.FirstOrDefaultAsync(stat => stat.AssignmentStatusId == targetAssignmentStatus.assignmentStatusID);

            contextAssignmentStatus.AssignmentStatusName = targetAssignmentStatus.assignmentStatusName;
            contextAssignmentStatus.AssignmentStatusDescription = targetAssignmentStatus.assignmentStatusDescription;

            santaContext.AssignmentStatus.Update(contextAssignmentStatus);
        }

        public async Task DeleteAssignmentStatusByID(Guid assignmentStatusID)
        {
            Entities.AssignmentStatus contextAssignmentStatus = await santaContext.AssignmentStatus.FirstOrDefaultAsync(stat => stat.AssignmentStatusId == assignmentStatusID);

            santaContext.Remove(contextAssignmentStatus);
        }

        #endregion

        #region Profile
        public async Task<Logic.Objects.Profile> GetProfileByEmailAsync(string email)
        {
            Logic.Objects.Profile logicProfile = Mapper.MapProfile(await santaContext.Client
                /* Assignment information and surveys */
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(clXref => clXref.RecipientClient.SurveyResponse.Where(r => r.SurveyQuestion.SenderCanView == true))
                        .ThenInclude(sr => sr.Survey.EventType)
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(clXref => clXref.RecipientClient.SurveyResponse)
                        .ThenInclude(sr => sr.SurveyQuestion.SurveyQuestionXref)

                /* Assignment event types */
                .Include(r => r.ClientRelationXrefSenderClient)
                    .ThenInclude(e => e.EventType)

                /* Assignment Statuses */
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(xref => xref.AssignmentStatus)
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(xref => xref.AssignmentStatus)

                /* Sender/Assignment Tags */
                .Include(c => c.ClientRelationXrefSenderClient)
                    .ThenInclude(xref => xref.SenderClient.ClientTagXref)
                        .ThenInclude(txr => txr.Tag)
                .Include(c => c.ClientRelationXrefRecipientClient)
                    .ThenInclude(xref => xref.RecipientClient.ClientTagXref)
                        .ThenInclude(txr => txr.Tag)

                /* Profile approval status */
                .Include(s => s.ClientStatus)

                /* Profile survey responses aand event types */
                .Include(c => c.SurveyResponse)
                    .ThenInclude(s => s.SurveyQuestion.SurveyQuestionXref)
                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.Survey.EventType)
                .Where(c => c.Email == email)
                .FirstOrDefaultAsync());

            List<Response> responsesToRemove = new List<Response>();
            // For each recipient in the profile
            foreach (ProfileRecipient recipient in logicProfile.assignments)
            {
                // And for each response that recipient gave
                foreach (Response response in recipient.responses)
                {
                    // If the recipients event does not match the responses event
                    if (recipient.recipientEvent.eventTypeID != response.responseEvent.eventTypeID)
                    {
                        // Add it to the list of responses to remove from thsi assignment
                        responsesToRemove.Add(response);
                    }
                }
                // Then once that is done, remove all the responses from that recipient that were gathered, and reset the list for any others
                foreach (Response response in responsesToRemove)
                {
                    recipient.responses.Remove(response);
                }
                responsesToRemove.Clear();
            }


            return logicProfile;
        }
        #endregion

        #region Tag
        public async Task CreateTag(Logic.Objects.Tag newTag)
        {
            try
            {
                Data.Entities.Tag contextTag = Mapper.MapTag(newTag);
                await santaContext.Tag.AddAsync(contextTag);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task CreateClientTagRelationByID(Guid clientID, Guid tagID)
        {
            try
            {
                Data.Entities.ClientTagXref contextClientRelationXref = new ClientTagXref()
                {
                    ClientTagXrefId = Guid.NewGuid(),
                    ClientId = clientID,
                    TagId = tagID
                };
                await santaContext.ClientTagXref.AddAsync(contextClientRelationXref);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<Logic.Objects.Tag> GetTagByIDAsync(Guid tagID)
        {
            try
            {
                Logic.Objects.Tag logicTag = Mapper.MapTag(await santaContext.Tag
                    .Include(t => t.ClientTagXref)
                    .FirstOrDefaultAsync(t => t.TagId == tagID));
                return logicTag;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<List<Logic.Objects.Tag>> GetAllTags()
        {
            try
            {
                List<Logic.Objects.Tag> logicTags = (await santaContext.Tag
                    .Include(t => t.ClientTagXref)
                    .ToListAsync())
                    .Select(Mapper.MapTag)
                    .ToList();

                return logicTags;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task UpdateTagNameByIDAsync(Logic.Objects.Tag targetLogicTag)
        {
            try
            {
                Data.Entities.Tag contextOldTag = await santaContext.Tag.FirstOrDefaultAsync(t => t.TagId == targetLogicTag.tagID);

                contextOldTag.TagName = targetLogicTag.tagName;

                santaContext.Tag.Update(contextOldTag);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task DeleteClientTagRelationshipByID(Guid clientID, Guid tagID)
        {
            try
            {
                Data.Entities.ClientTagXref contextClientTagXref = await santaContext.ClientTagXref.FirstOrDefaultAsync(tx => tx.ClientId == clientID && tx.TagId == tagID);
                santaContext.ClientTagXref.Remove(contextClientTagXref);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task DeleteTagByIDAsync(Guid tagID)
        {
            try
            {
                Data.Entities.Tag contextTag = await santaContext.Tag.FirstOrDefaultAsync(t => t.TagId == tagID);
                santaContext.Tag.Remove(contextTag);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion

        #region Message
        public async Task CreateMessage(Message newMessage)
        {
            Data.Entities.ChatMessage contextMessage = Mapper.MapMessage(newMessage);
            await santaContext.ChatMessage.AddAsync(contextMessage);
        }
        public async Task<List<Message>> GetAllMessages()
        {
            List<Logic.Objects.Message> logicMessageList = (await santaContext.ChatMessage
                .Include(s => s.MessageSenderClient)
                .Include(r => r.MessageReceiverClient)
                .Include(x => x.ClientRelationXref)
                .ToListAsync())
                .Select(Mapper.MapMessage).ToList();

            return logicMessageList;
        }
        public async Task<Message> GetMessageByIDAsync(Guid chatMessageID)
        {
            ChatMessage contextMessage = await santaContext.ChatMessage
                .Include(r => r.MessageReceiverClient)
                .Include(s => s.MessageSenderClient)
                .Where(m => m.ChatMessageId == chatMessageID)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            Logic.Objects.Message logicMessage = Mapper.MapMessage(contextMessage);
            return logicMessage;
        }
        public async Task UpdateMessageByIDAsync(Message targetMessage)
        {
            Entities.ChatMessage contextMessage = await santaContext.ChatMessage.FirstOrDefaultAsync(m => m.ChatMessageId == targetMessage.chatMessageID);

            contextMessage.IsMessageRead = targetMessage.isMessageRead;

            santaContext.ChatMessage.Update(contextMessage);
        }

        #region Message Histories
        public async Task<List<MessageHistory>> GetAllChatHistories(Logic.Objects.Client subjectClient)
        {
            List<MessageHistory> listLogicMessageHistory = new List<MessageHistory>();

            List<Entities.Client> contextClients = await santaContext.Client.Include(c => c.ClientStatus).Where(c => c.ClientStatus.StatusDescription != Constants.AWAITING_STATUS && c.ClientStatus.StatusDescription != Constants.DENIED_STATUS).ToListAsync();

            List<ClientRelationXref> contextRelationshipsWithChats = await santaContext.ClientRelationXref
                .Include(r => r.SenderClient.ClientStatus)
                .Include(r => r.RecipientClient.ClientStatus)

                .Include(r => r.EventType)
                .Include(r => r.EventType.Survey)

                .Include(r => r.AssignmentStatus)

                .Include(r => r.ChatMessage)
                    .ThenInclude(cm => cm.MessageReceiverClient)
                .Include(r => r.ChatMessage)
                    .ThenInclude(cm => cm.MessageSenderClient)

                .AsNoTracking()
                .ToListAsync();

            List<ChatMessage> contextGeneralChatMessages = await santaContext.ChatMessage
                .Where(m => m.ClientRelationXrefId == null)
                .Include(s => s.MessageSenderClient)
                .Include(r => r.MessageReceiverClient)
                .AsNoTracking()
                .OrderBy(dt => dt.DateTimeSent)
                .ToListAsync();

            // All the chats for assignments
            foreach (ClientRelationXref contextRelationship in contextRelationshipsWithChats.Where(r => r.SenderClient.ClientStatus.StatusDescription == Constants.APPROVED_STATUS))
            {
                listLogicMessageHistory.Add(Mapper.MapHistoryInformation(contextRelationship, subjectClient));
            }
            // All the general chats
            foreach (Entities.Client contextClient in contextClients)
            {
                listLogicMessageHistory.Add(Mapper.MapHistoryInformation(contextClient, contextGeneralChatMessages.Where(m => m.MessageSenderClientId == contextClient.ClientId || m.MessageReceiverClientId == contextClient.ClientId).ToList(), subjectClient));
            }

            return listLogicMessageHistory.OrderBy(h => h.conversationClient.clientNickname).ToList();
        }
        public async Task<List<MessageHistory>> GetAllChatHistoriesBySubjectIDAsync(Logic.Objects.Client subjectClient)
        {
            List<MessageHistory> listLogicMessageHistory = new List<MessageHistory>();
            List<ClientRelationXref> XrefList = await santaContext.ClientRelationXref.Where(x => x.SenderClientId == subjectClient.clientID).ToListAsync();


            foreach (ClientRelationXref relationship in XrefList)
            {
                MessageHistory logicHistory = new MessageHistory();
                logicHistory = await GetChatHistoryByXrefIDAndSubjectIDAsync(relationship.ClientRelationXrefId, subjectClient);
                listLogicMessageHistory.Add(logicHistory);
            }

            return listLogicMessageHistory;
        }
        public async Task<MessageHistory> GetChatHistoryByXrefIDAndSubjectIDAsync(Guid clientRelationXrefID, Logic.Objects.Client subjectClient)
        {
            MessageHistory logicHistory = new MessageHistory();
            ClientRelationXref contextRelationship = await santaContext.ClientRelationXref
                .Include(r => r.EventType)
                .Include(r => r.SenderClient)
                .Include(r => r.RecipientClient)
                .Include(r => r.AssignmentStatus)
                .Include(r => r.ChatMessage)
                    .ThenInclude(cm => cm.MessageReceiverClient)
                .Include(r => r.ChatMessage)
                    .ThenInclude(cm => cm.MessageSenderClient)
                .Where(x => x.ClientRelationXrefId == clientRelationXrefID)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return Mapper.MapHistoryInformation(contextRelationship, subjectClient);
        }
        public async Task<MessageHistory> GetGeneralChatHistoryBySubjectIDAsync(Logic.Objects.Client conversationClient, Logic.Objects.Client subjectClient)
        {
            MessageHistory logicHistory = new MessageHistory();
            List<Entities.ChatMessage> contextListMessages = await santaContext.ChatMessage
                .Where(m => m.ClientRelationXrefId == null && (m.MessageSenderClientId == conversationClient.clientID || m.MessageReceiverClientId == conversationClient.clientID))
                .Include(s => s.MessageSenderClient)
                .Include(r => r.MessageReceiverClient)
                .AsNoTracking()
                .OrderBy(dt => dt.DateTimeSent)
                .ToListAsync();

            return Mapper.MapHistoryInformation(conversationClient, contextListMessages, subjectClient);
        }
        #endregion

        #endregion

        #region ClientStatus
        public async Task<List<Status>> GetAllClientStatus()
        {
            List<Logic.Objects.Status> logicStatusList = (await santaContext.ClientStatus.ToListAsync()).Select(Mapper.MapStatus).ToList();
            return logicStatusList;
        }
        public async Task<Status> GetClientStatusByID(Guid clientStatusID)
        {
            Logic.Objects.Status logicStatus = Mapper.MapStatus(await santaContext.ClientStatus
                .FirstOrDefaultAsync(s => s.ClientStatusId == clientStatusID));
            return logicStatus;
        }
        public async Task CreateStatusAsync(Status newStatus)
        {
            Data.Entities.ClientStatus contextStatus = Mapper.MapStatus(newStatus);
            await santaContext.ClientStatus.AddAsync(contextStatus);
        }
        public async Task UpdateStatusByIDAsync(Status changedLogicStatus)
        {
            ClientStatus targetStatus = await santaContext.ClientStatus.FirstOrDefaultAsync(s => s.ClientStatusId == changedLogicStatus.statusID);
            if (targetStatus == null)
            {
                throw new Exception("Client Status was not found. Update failed for status");
            }
            targetStatus.StatusDescription = changedLogicStatus.statusDescription;
            santaContext.ClientStatus.Update(targetStatus);
        }
        public async Task DeleteStatusByIDAsync(Guid clientStatusID)
        {
            ClientStatus contextStatus = await santaContext.ClientStatus.FirstOrDefaultAsync(s => s.ClientStatusId == clientStatusID);
            santaContext.ClientStatus.Remove(contextStatus);
        }
        #endregion

        #region Event
        public async Task CreateEventAsync(Event newEvent)
        {
            Data.Entities.EventType contextEvent = Mapper.MapEvent(newEvent);
            await santaContext.EventType.AddAsync(contextEvent);
        }
        public async Task DeleteEventByIDAsync(Guid eventID)
        {
            Data.Entities.EventType contextEvent = await santaContext.EventType.FirstOrDefaultAsync(e => e.EventTypeId == eventID);
            santaContext.EventType.Remove(contextEvent);
        }
        public async Task<List<Event>> GetAllEvents()
        {
            List<Logic.Objects.Event> eventList = (await santaContext.EventType
                .Include(e => e.ClientRelationXref)
                .Include(e => e.Survey)
                .ToListAsync())
                .Select(Mapper.MapEvent)
                .ToList();
            return eventList;
        }
        public async Task<Event> GetEventByIDAsync(Guid eventID)
        {
            Logic.Objects.Event logicEvent = Mapper.MapEvent(await santaContext.EventType
                .Include(e => e.ClientRelationXref)
                .Include(e => e.Survey)
                .FirstOrDefaultAsync(e => e.EventTypeId == eventID));
            return logicEvent;
        }
        public async Task<Event> GetEventByNameAsync(string eventName)
        {
            Logic.Objects.Event logicEvent = Mapper.MapEvent(await santaContext.EventType
                .Include(e => e.Survey)
                .Include(e => e.ClientRelationXref)
                .FirstOrDefaultAsync(e => e.EventDescription == eventName));
            return logicEvent;
        }
        public async Task UpdateEventByIDAsync(Event targetEvent)
        {
            Data.Entities.EventType oldContextEvent = await santaContext.EventType.FirstOrDefaultAsync(e => e.EventTypeId == targetEvent.eventTypeID);

            oldContextEvent.EventDescription = targetEvent.eventDescription;
            oldContextEvent.IsActive = targetEvent.active;

            santaContext.Update(oldContextEvent);
        }
        #endregion

        #region Survey
        public async Task CreateSurveyAsync(Logic.Objects.Survey newSurvey)
        {
            Data.Entities.Survey contextSurvey = Mapper.MapSurvey(newSurvey);
            await santaContext.Survey.AddAsync(contextSurvey);
        }
        public async Task<List<Logic.Objects.Survey>> GetAllSurveys()
        {
            List<Logic.Objects.Survey> surveyList = (await santaContext.Survey
                .Include(s => s.SurveyQuestionXref)
                    .ThenInclude(sqx => sqx.SurveyQuestion.SurveyQuestionOptionXref)
                        .ThenInclude(so => so.SurveyOption)
                .Include(s => s.SurveyQuestionXref)
                    .ThenInclude(sqx => sqx.SurveyQuestion.SurveyQuestionXref)

                .ToListAsync())

                .Select(Mapper.MapSurvey).ToList();

            return surveyList;
        }
        public async Task<Logic.Objects.Survey> GetSurveyByID(Guid surveyId)
        {
            Logic.Objects.Survey logicSurvey = Mapper.MapSurvey(await santaContext.Survey
                .Include(s => s.SurveyQuestionXref)
                    .ThenInclude(sqx => sqx.SurveyQuestion.SurveyQuestionOptionXref)
                        .ThenInclude(sqox => sqox.SurveyOption)
                .Include(s => s.SurveyQuestionXref)
                    .ThenInclude(sqx => sqx.SurveyQuestion.SurveyQuestionXref)

                .Where(s => s.SurveyId == surveyId)
                .AsNoTracking()
                .FirstOrDefaultAsync());
            return logicSurvey;
        }
        public async Task UpdateSurveyByIDAsync(Logic.Objects.Survey targetSurvey)
        {
            Data.Entities.Survey contextOldSurvey = await santaContext.Survey.FirstOrDefaultAsync(s => s.SurveyId == targetSurvey.surveyID);
            contextOldSurvey.SurveyDescription = targetSurvey.surveyDescription;
            contextOldSurvey.IsActive = targetSurvey.active;
            santaContext.Update(contextOldSurvey);
        }
        public async Task DeleteSurveyByIDAsync(Guid surveyID)
        {
            Data.Entities.Survey contextSurvey = await santaContext.Survey.FirstOrDefaultAsync(s => s.SurveyId == surveyID);
            santaContext.Survey.Remove(contextSurvey);
        }
        public async Task DeleteSurveyQuestionXrefBySurveyIDAndQuestionID(Guid surveyID, Guid surveyQuestionID)
        {
            Data.Entities.SurveyQuestionXref contextSurveyQuestionXref = await santaContext.SurveyQuestionXref.FirstOrDefaultAsync(sqx => sqx.SurveyId == surveyID && sqx.SurveyQuestionId == surveyQuestionID);
            santaContext.SurveyQuestionXref.Remove(contextSurveyQuestionXref);
        }
        #endregion

        #region SurveyOption

        public async Task<List<Option>> GetAllSurveyOption()
        {
            List<Option> listLogicSurveyOption = (await santaContext.SurveyOption
                .Include(s => s.SurveyQuestionOptionXref)
                .Include(s => s.SurveyResponse)
                .ToListAsync())
                .Select(Mapper.MapSurveyOption)
                .ToList();
            return listLogicSurveyOption;
        }

        public async Task<Option> GetSurveyOptionByIDAsync(Guid surveyOptionID)
        {
            Option logicOption = Mapper.MapSurveyOption(await santaContext.SurveyOption
                .Include(s => s.SurveyResponse)
                .Include(s => s.SurveyQuestionOptionXref)
                .FirstOrDefaultAsync(so => so.SurveyOptionId == surveyOptionID));
            return logicOption;
        }

        public async Task CreateSurveyOptionAsync(Option newSurveyOption)
        {
            Data.Entities.SurveyOption contextQuestionOption = Mapper.MapSurveyOption(newSurveyOption);
            await santaContext.SurveyOption.AddAsync(contextQuestionOption);
        }

        public async Task UpdateSurveyOptionByIDAsync(Option targetSurveyOption)
        {
            Data.Entities.SurveyOption oldOption = await santaContext.SurveyOption.FirstOrDefaultAsync(o => o.SurveyOptionId == targetSurveyOption.surveyOptionID);

            oldOption.DisplayText = targetSurveyOption.displayText;
            oldOption.SurveyOptionValue = targetSurveyOption.surveyOptionValue;

            santaContext.SurveyOption.Update(oldOption);
        }
        
        public async Task DeleteSurveyOptionByIDAsync(Guid surveyOptionID)
        {
            santaContext.SurveyOption.Remove(await santaContext.SurveyOption.FirstOrDefaultAsync(o => o.SurveyOptionId == surveyOptionID));
        }
        #endregion

        #region SurveyQuestionOptionXref
        public async Task CreateSurveyQuestionOptionXrefAsync(Option newQuestionOption, Guid surveyQuestionID, bool isActive, int sortOrder)
        {
            try
            {
                Data.Entities.SurveyQuestionOptionXref contextQuestionOptionXref = Mapper.MapQuestionOptionXref(newQuestionOption);
                contextQuestionOptionXref.SurveyQuestionId = surveyQuestionID;
                contextQuestionOptionXref.IsActive = isActive;
                contextQuestionOptionXref.SortOrder = sortOrder;
                await santaContext.SurveyQuestionOptionXref.AddAsync(contextQuestionOptionXref);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

        }
        #endregion

        #region Question
        public async Task<List<Question>> GetAllSurveyQuestions()
        {
            List<Question> listLogicQuestion = (await santaContext.SurveyQuestion
                .Include(sq => sq.SurveyResponse)
                .Include(sq => sq.SurveyQuestionOptionXref)
                    .ThenInclude(so => so.SurveyOption)
                .Include(sq => sq.SurveyQuestionXref)
                .ToListAsync())

                .Select(Mapper.MapQuestion).ToList();
            return listLogicQuestion;
        }
        public async Task<Question> GetSurveyQuestionByIDAsync(Guid questionID)
        {
            Logic.Objects.Question logicQuestion = Mapper.MapQuestion(await santaContext.SurveyQuestion
                .Include(sq => sq.SurveyResponse)
                .Include(sq => sq.SurveyQuestionOptionXref)
                    .ThenInclude(sqoxr => sqoxr.SurveyOption)
                .Include(sq => sq.SurveyQuestionXref)
                .FirstOrDefaultAsync(q => q.SurveyQuestionId == questionID));
            return logicQuestion;
        }
        public async Task CreateSurveyQuestionAsync(Question newQuestion)
        {
            Entities.SurveyQuestion contextQuestion = Mapper.MapQuestion(newQuestion);
            await santaContext.SurveyQuestion.AddAsync(contextQuestion);
        }
        public async Task CreateSurveyQuestionXrefAsync(Guid surveyID, Guid questionID)
        {
            Data.Entities.SurveyQuestionXref contextQuestionXref = new SurveyQuestionXref()
            {
                SurveyQuestionXrefId = Guid.NewGuid(),
                SurveyQuestionId = questionID,
                SurveyId = surveyID,
                SortOrder = 0,
                IsActive = true
            };
            await santaContext.SurveyQuestionXref.AddAsync(contextQuestionXref);
        }
        public async Task UpdateSurveyQuestionByIDAsync(Question targetQuestion)
        {
            Data.Entities.SurveyQuestion oldQuestion = await santaContext.SurveyQuestion.FirstOrDefaultAsync(q => q.SurveyQuestionId == targetQuestion.questionID);

            oldQuestion.QuestionText = targetQuestion.questionText;
            oldQuestion.IsSurveyOptionList = targetQuestion.isSurveyOptionList;
            oldQuestion.SenderCanView = targetQuestion.senderCanView;

            santaContext.SurveyQuestion.Update(oldQuestion);
        }
        public async Task DeleteSurveyQuestionByIDAsync(Guid surveyQuestionID)
        {
            santaContext.SurveyQuestion.Remove(await santaContext.SurveyQuestion.FirstOrDefaultAsync(q => q.SurveyQuestionId == surveyQuestionID));
        }
        #endregion

        #region Response

        public async Task<Logic.Objects.Response> GetSurveyResponseByIDAsync(Guid surveyResponseID)
        {
            Logic.Objects.Response logicResponse = Mapper.MapResponse(await santaContext.SurveyResponse
                .Include(sr => sr.Survey)
                    .ThenInclude(s => s.EventType)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionXref)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionOptionXref)
                    .ThenInclude(sqox => sqox.SurveyOption)
                .FirstOrDefaultAsync(r => r.SurveyResponseId == surveyResponseID));
            return logicResponse;
        }
        public async Task<List<Logic.Objects.Response>> GetAllSurveyResponsesByClientID(Guid clientID)
        {
            List<Response> listLogicResponse = (await santaContext.SurveyResponse
                .Include(sr => sr.Survey)
                    .ThenInclude(s => s.EventType)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionXref)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionOptionXref)
                    .ThenInclude(sqox => sqox.SurveyOption)
                .Where(r => r.ClientId == clientID)
                .ToListAsync())
                .Select(Mapper.MapResponse)
                .ToList();
            return listLogicResponse;
        }
        public async Task<List<Logic.Objects.Response>> GetAllSurveyResponses()
        {
            List<Response> listLogicResponse = (await santaContext.SurveyResponse
                .Include(sr => sr.Survey)
                    .ThenInclude(s => s.EventType)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionXref)
                .Include(sr => sr.SurveyQuestion.SurveyQuestionOptionXref)
                    .ThenInclude(sqox => sqox.SurveyOption)
                .ToListAsync())
                .Select(Mapper.MapResponse)
                .ToList();
            return listLogicResponse;
        }

        public async Task CreateSurveyResponseAsync(Response newResponse)
        {
            Data.Entities.SurveyResponse contextResponse = Mapper.MapResponse(newResponse);
            await santaContext.SurveyResponse.AddAsync(contextResponse);
        }
        public async Task DeleteSurveyResponseByIDAsync(Guid surveyResponseID)
        {
            Data.Entities.SurveyResponse contextResponse = await santaContext.SurveyResponse.FirstOrDefaultAsync(r => r.SurveyResponseId == surveyResponseID);
            santaContext.Remove(contextResponse);
        }
        public async Task UpdateSurveyResponseByIDAsync(Response targetResponse)
        {
            Data.Entities.SurveyResponse contextOldResponse = await santaContext.SurveyResponse.FirstOrDefaultAsync(r => r.SurveyResponseId == targetResponse.surveyResponseID);

            contextOldResponse.ResponseText = targetResponse.responseText;

            santaContext.SurveyResponse.Update(contextOldResponse);
        }
        #endregion

        #region Board Entry
        public async Task CreateBoardEntryAsync(Logic.Objects.BoardEntry newEntry)
        {
            await santaContext.AddAsync(Mapper.MapBoardEntry(newEntry));
        }

        public async Task<List<Logic.Objects.BoardEntry>> GetAllBoardEntriesAsync()
        {
            List<Logic.Objects.BoardEntry> logicListBoardEntries = (await santaContext.BoardEntry
                .Include(b => b.EntryType)
                .ToListAsync())
                .Select(Mapper.MapBoardEntry)
                .ToList();
            return logicListBoardEntries;
        }

        public async Task<Logic.Objects.BoardEntry> GetBoardEntryByIDAsync(Guid boardEntryID)
        {
            Logic.Objects.BoardEntry logicBoardEntry = Mapper.MapBoardEntry(await santaContext.BoardEntry
                .Include(b => b.EntryType)
                .FirstOrDefaultAsync(b => b.BoardEntryId == boardEntryID));
            return logicBoardEntry;
        }

        public async Task<Logic.Objects.BoardEntry> GetBoardEntryByThreadAndPostNumberAsync(int threadNumber, int postNumber)
        {
            Logic.Objects.BoardEntry logicBoardEntry = Mapper.MapBoardEntry(await santaContext.BoardEntry
                .Include(b => b.EntryType)
                .FirstOrDefaultAsync(b => b.ThreadNumber == threadNumber && b.PostNumber == postNumber));
            return logicBoardEntry;
        }
        public async Task UpdateBoardEntryPostNumberAsync(Logic.Objects.BoardEntry newEntry)
        {
            Data.Entities.BoardEntry contextBoardEntry = await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.BoardEntryId == newEntry.boardEntryID);

            contextBoardEntry.PostNumber = newEntry.postNumber;

            santaContext.BoardEntry.Update(contextBoardEntry);
        }
        public async Task UpdateBoardEntryThreadNumberAsync(Logic.Objects.BoardEntry newEntry)
        {
            Data.Entities.BoardEntry contextBoardEntry = await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.BoardEntryId == newEntry.boardEntryID);

            contextBoardEntry.ThreadNumber = newEntry.threadNumber;

            santaContext.BoardEntry.Update(contextBoardEntry);
        }

        public async Task UpdateBoardEntryPostDescriptionAsync(Logic.Objects.BoardEntry newEntry)
        {
            Data.Entities.BoardEntry contextBoardEntry = await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.BoardEntryId == newEntry.boardEntryID);

            contextBoardEntry.PostDescription = newEntry.postDescription;

            santaContext.BoardEntry.Update(contextBoardEntry);
        }

        public async Task UpdateBoardEntryTypeAsync(Logic.Objects.BoardEntry newEntry)
        {
            Data.Entities.BoardEntry contextBoardEntry = await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.BoardEntryId == newEntry.boardEntryID);

            contextBoardEntry.EntryTypeId = newEntry.entryType.entryTypeID;

            santaContext.BoardEntry.Update(contextBoardEntry);
        }

        public async Task DeleteBoardEntryByIDAsync(Guid boardEntryID)
        {
            Entities.BoardEntry contextBoardEntry = await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.BoardEntryId == boardEntryID);
            santaContext.Remove(contextBoardEntry);
        }

        #endregion

        #region Entry Type
        public async Task CreateEntryTypeAsync(Logic.Objects.EntryType newEntryType)
        {
            await santaContext.EntryType.AddAsync(Mapper.MapEntryType(newEntryType));
        }

        public async Task<List<Logic.Objects.EntryType>> GetAllEntryTypesAsync()
        {
            List<Logic.Objects.EntryType> listLogicEntryType = (await santaContext.EntryType
                .ToListAsync())
                .Select(Mapper.MapEntryType)
                .ToList();
            return listLogicEntryType;
        }

        public async Task<Logic.Objects.EntryType> GetEntryTypeByIDAsync(Guid entryTypeID)
        {
            Logic.Objects.EntryType logicEntryType = Mapper.MapEntryType(await santaContext.EntryType.FirstOrDefaultAsync(e => e.EntryTypeId == entryTypeID));
            return logicEntryType;
        }

        public async Task UpdateEntryTypeName(Logic.Objects.EntryType updatedEntryType)
        {
            Entities.EntryType contextEntryType = await santaContext.EntryType.FirstOrDefaultAsync(e => e.EntryTypeId == updatedEntryType.entryTypeID);

            contextEntryType.EntryTypeName = updatedEntryType.entryTypeName;

            santaContext.EntryType.Update(contextEntryType);
        }

        public async Task UpdateEntryTypeDescription(Logic.Objects.EntryType updatedEntryType)
        {
            Entities.EntryType contextEntryType = await santaContext.EntryType.FirstOrDefaultAsync(e => e.EntryTypeId == updatedEntryType.entryTypeID);

            contextEntryType.EntryTypeDescription = updatedEntryType.entryTypeDescription;

            santaContext.EntryType.Update(contextEntryType);
        }

        public async Task DeleteEntryTypeByID(Guid entryTypeID)
        {
            Entities.EntryType contextEntryType = await santaContext.EntryType.FirstOrDefaultAsync(e => e.EntryTypeId == entryTypeID);

            santaContext.EntryType.Remove(contextEntryType);
        }
        #endregion

        #region Utility
        public async Task SaveAsync()
        {
            await santaContext.SaveChangesAsync();
        }
        public async Task<List<AllowedAssignmentMeta>> GetAllAllowedAssignmentsByID(Guid clientID, Guid eventTypeID)
        {
            Logic.Objects.Client logicClient = await GetClientByIDAsync(clientID);
            List<Logic.Objects.Client> allClients = await GetAllClients();
            List<Event> allEvents = await GetAllEvents();
            List<AllowedAssignmentMeta> allowedAssignments = new List<AllowedAssignmentMeta>();


            foreach (Logic.Objects.Client potentialAssignment in allClients)
            {
                // If the client doesnt have any assignments that match the potential assignment and eventType, the potential assignment is approved, and the potential assignment is not the current client
                if (!logicClient.assignments.Any<RelationshipMeta>(c => c.relationshipClient.clientId == potentialAssignment.clientID && c.eventType.eventTypeID == eventTypeID) && potentialAssignment.clientStatus.statusDescription == Constants.APPROVED_STATUS && potentialAssignment.clientID != clientID)
                {
                    AllowedAssignmentMeta assignment = Mapper.MapAllowedAssignmentMeta(potentialAssignment);
                    assignment.clientEvents = allEvents.Where(e => potentialAssignment.responses.Any(r => r.responseEvent.eventTypeID == e.eventTypeID)).ToList();
                    allowedAssignments.Add(assignment);
                }
            }
            return allowedAssignments;
        }

        public async Task<List<Logic.Objects.Client>> SearchClientByQuery(SearchQueries searchQuery)
        {
            List<Logic.Objects.Client> allClientList = await GetAllClients();
            List<Logic.Objects.Client> matchingClients = new List<Logic.Objects.Client>();

            if (searchQuery.isHardSearch)
            {
                matchingClients = allClientList
                    .Where(c => !searchQuery.tags.Any() || searchQuery.tags.All(queryTagID => c.tags.Any(clientTag => clientTag.tagID == queryTagID)))
                    .Where(c => !searchQuery.statuses.Any() || searchQuery.statuses.All(queryStatusID => c.clientStatus.statusID == queryStatusID))
                    .Where(c => !searchQuery.events.Any() || searchQuery.events.All(queryEventID => c.responses.Any(r => r.responseEvent.eventTypeID == queryEventID)))
                    .Where(c => !searchQuery.names.Any() || searchQuery.names.All(queryName => c.clientName == queryName))
                    .Where(c => !searchQuery.nicknames.Any() || searchQuery.nicknames.All(queryNickname => c.nickname == queryNickname)).ToList();
            }
            else
            {
                matchingClients = allClientList
                    .Where(c => !searchQuery.tags.Any() || searchQuery.tags.Any(queryTagID => c.tags.Any(t => t.tagID == queryTagID)))
                    .Where(c => !searchQuery.statuses.Any() || searchQuery.statuses.Any(queryStatusID => c.clientStatus.statusID == queryStatusID))
                    .Where(c => !searchQuery.events.Any() || searchQuery.events.Any(queryEventID => c.responses.Any(r => r.responseEvent.eventTypeID == queryEventID)))
                    .Where(c => !searchQuery.names.Any() || searchQuery.names.Any(queryName => c.clientName == queryName))
                    .Where(c => !searchQuery.nicknames.Any() || searchQuery.nicknames.Any(queryNickname => c.nickname == queryNickname)).ToList();
            }

            return matchingClients;
        }
        #endregion
    }
}
