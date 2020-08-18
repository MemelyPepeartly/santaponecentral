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
            try
            {
                Entities.Client contextClient = Mapper.MapClient(newClient);
                await santaContext.Client.AddAsync(contextClient);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task CreateClientRelationByID(Guid senderClientID, Guid recipientClientID, Guid eventTypeID)
        {
            try
            {
                Data.Entities.ClientRelationXref contexRelation = new ClientRelationXref()
                {
                    ClientRelationXrefId = Guid.NewGuid(),
                    SenderClientId = senderClientID,
                    RecipientClientId = recipientClientID,
                    EventTypeId = eventTypeID,
                    // Value being posted should be false to start by default
                    Completed = false
                };
                await santaContext.ClientRelationXref.AddAsync(contexRelation);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<List<Logic.Objects.Client>> GetAllClients()
        {
            try
            {
                List<Logic.Objects.Client> clientList = new List<Logic.Objects.Client>();
                clientList = (await santaContext.Client
                    .Include(r => r.ClientRelationXrefRecipientClient)
                        .ThenInclude(crx => crx.RecipientClient)
                    .Include(sc => sc.ClientRelationXrefSenderClient)
                        .ThenInclude(crx => crx.SenderClient)
                    .Include(xr => xr.ClientRelationXrefRecipientClient)
                        .ThenInclude(m => m.ChatMessage)
                    .Include(xr => xr.ClientRelationXrefSenderClient)
                        .ThenInclude(m => m.ChatMessage)
                    .Include(tx => tx.ClientTagXref)
                        .ThenInclude(t => t.Tag)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.SurveyQuestion)
                            .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                                .ThenInclude(sqox => sqox.SurveyOption)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.Survey)
                            .ThenInclude(s => s.EventType)
                    .Include(s => s.ClientStatus).ToListAsync())
                    .Select(Mapper.MapClient).ToList();

                return clientList;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<Logic.Objects.Client> GetClientByIDAsync(Guid clientId)
        {
            try
            {
                Logic.Objects.Client logicClient = Mapper.MapClient(await santaContext.Client
                    .Include(s => s.ClientRelationXrefSenderClient)
                        .ThenInclude(u => u.SenderClient)
                    .Include(r => r.ClientRelationXrefRecipientClient)
                        .ThenInclude(u => u.RecipientClient)
                    .Include(xr => xr.ClientRelationXrefRecipientClient)
                        .ThenInclude(m => m.ChatMessage)
                    .Include(xr => xr.ClientRelationXrefSenderClient)
                        .ThenInclude(m => m.ChatMessage)
                    .Include(tx => tx.ClientTagXref)
                        .ThenInclude(t => t.Tag)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.SurveyQuestion)
                            .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                                .ThenInclude(sqox => sqox.SurveyOption)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.Survey)
                            .ThenInclude(s => s.EventType)
                    .Include(s => s.ClientStatus)
                    .FirstOrDefaultAsync(c => c.ClientId == clientId));

                return logicClient;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<Logic.Objects.Client> GetClientByEmailAsync(string clientEmail)
        {
            try
            {
                Logic.Objects.Client logicClient = Mapper.MapClient(await santaContext.Client
                    .Include(s => s.ClientRelationXrefSenderClient)
                        .ThenInclude(u => u.SenderClient)
                    .Include(r => r.ClientRelationXrefRecipientClient)
                        .ThenInclude(u => u.RecipientClient)
                    .Include(xr => xr.ClientRelationXrefRecipientClient)
                        .ThenInclude(m => m.ChatMessage)
                    .Include(xr => xr.ClientRelationXrefSenderClient)
                        .ThenInclude(m => m.ChatMessage)
                    .Include(tx => tx.ClientTagXref)
                        .ThenInclude(t => t.Tag)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.SurveyQuestion)
                            .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                                .ThenInclude(sqox => sqox.SurveyOption)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.Survey)
                            .ThenInclude(s => s.EventType)
                    .Include(s => s.ClientStatus)
                    .FirstOrDefaultAsync(c => c.Email == clientEmail));

                return logicClient;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task UpdateClientByIDAsync(Logic.Objects.Client targetLogicClient)
        {
            try
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

                santaContext.Client.Update(contextOldClient);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task UpdateClientRelationCompletedStatusByID(Guid senderID, Guid recipientID, Guid eventTypeID, bool targetCompletedStatus)
        {
            try
            {
                ClientRelationXref contextRelationship = await santaContext.ClientRelationXref.FirstOrDefaultAsync(crxf => crxf.RecipientClientId == recipientID && crxf.SenderClientId == senderID && crxf.EventTypeId == eventTypeID);

                contextRelationship.Completed = targetCompletedStatus;

                santaContext.ClientRelationXref.Update(contextRelationship);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task DeleteClientByIDAsync(Guid clientID)
        {
            try
            {
                Data.Entities.Client contextClient = await santaContext.Client.FirstOrDefaultAsync(c => c.ClientId == clientID);

                santaContext.Client.Remove(contextClient);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task DeleteRecieverXref(Guid clientID, Guid recipientID, Guid eventID)
        {
            try
            {
                Data.Entities.ClientRelationXref contextRelation = await santaContext.ClientRelationXref.FirstOrDefaultAsync(r => r.SenderClientId == clientID && r.RecipientClientId == recipientID && r.EventTypeId == eventID);
                santaContext.ClientRelationXref.Remove(contextRelation);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion

        #region Profile
        public async Task<Logic.Objects.Profile> GetProfileByEmailAsync(string email)
        {
            try
            {
                Logic.Objects.Profile logicProfile = Mapper.MapProfile(await santaContext.Client
                    .Include(r => r.ClientRelationXrefSenderClient)
                        .ThenInclude(clXref => clXref.RecipientClient)
                            .ThenInclude(c => c.SurveyResponse.Where(r => r.SurveyQuestion.SenderCanView == true))
                                .ThenInclude(sr => sr.Survey)
                                    .ThenInclude(s => s.EventType)
                    .Include(r => r.ClientRelationXrefSenderClient)
                        .ThenInclude(clXref => clXref.RecipientClient)
                            .ThenInclude(c => c.SurveyResponse)
                                .ThenInclude(sr => sr.SurveyQuestion)
                    .Include(r => r.ClientRelationXrefSenderClient)
                        .ThenInclude(e => e.EventType)
                    .Include(r => r.ClientRelationXrefRecipientClient)
                    .Include(s => s.ClientStatus)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(s => s.SurveyQuestion)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.Survey)
                            .ThenInclude(s => s.EventType)
                    .FirstOrDefaultAsync(c => c.Email == email));
                
                return logicProfile;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
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
            try
            {
                Data.Entities.ChatMessage contextMessage = Mapper.MapMessage(newMessage);
                await santaContext.ChatMessage.AddAsync(contextMessage);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<List<Message>> GetAllMessages()
        {
            try
            {
                List<Logic.Objects.Message> logicMessageList = (await santaContext.ChatMessage
                    .Include(s => s.MessageSenderClient)
                    .Include(r => r.MessageReceiverClient)
                    .Include(x => x.ClientRelationXref)
                    .ToListAsync())
                    .Select(Mapper.MapMessage).ToList();

                return logicMessageList;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<Message> GetMessageByIDAsync(Guid chatMessageID)
        {
            try
            {
                var contextMessage = await santaContext.ChatMessage
                    .Include(r => r.MessageReceiverClient)
                    .Include(s => s.MessageSenderClient)
                    .FirstOrDefaultAsync(m => m.ChatMessageId == chatMessageID);
                Logic.Objects.Message logicMessage = Mapper.MapMessage(contextMessage);
                return logicMessage;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task UpdateMessageByIDAsync(Message targetMessage)
        {
            try
            {
                Entities.ChatMessage contextMessage = await santaContext.ChatMessage.FirstOrDefaultAsync(m => m.ChatMessageId == targetMessage.chatMessageID);

                contextMessage.IsMessageRead = targetMessage.isMessageRead;

                santaContext.ChatMessage.Update(contextMessage);

            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        #region Message Histories
        public async Task<List<MessageHistory>> GetAllChatHistories(Logic.Objects.Client subjectClient)
        {
            try
            {
                List<MessageHistory> listLogicMessageHistory = new List<MessageHistory>();
                List<Entities.Client> listContextClient = await santaContext.Client.Include(c => c.ClientStatus).Include(x => x.ClientRelationXrefRecipientClient).ToListAsync();

                foreach (Entities.Client client in listContextClient.Where(c => c.ClientStatus.StatusDescription == Constants.APPROVED_STATUS))
                {
                    if(client.ClientRelationXrefRecipientClient.Count() > 0)
                    {
                        MessageHistory logicGeneralHistory = await GetGeneralChatHistoryBySubjectIDAsync(Mapper.MapClient(client), subjectClient);
                        listLogicMessageHistory.Add(logicGeneralHistory);
                        foreach (ClientRelationXref relationship in client.ClientRelationXrefRecipientClient)
                        {
                            MessageHistory logicHistory = await GetChatHistoryByXrefIDAndSubjectIDAsync(relationship.ClientRelationXrefId, subjectClient);
                            listLogicMessageHistory.Add(logicHistory);
                        }
                    }
                    else
                    {
                        MessageHistory logicGeneralHistory = await GetGeneralChatHistoryBySubjectIDAsync(Mapper.MapClient(client), subjectClient);
                        listLogicMessageHistory.Add(logicGeneralHistory);
                    }
                }
                return listLogicMessageHistory.OrderBy(h => h.eventType.eventDescription).ToList();
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<List<MessageHistory>> GetAllChatHistoriesBySubjectIDAsync(Logic.Objects.Client subjectClient)
        {
            try
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
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<MessageHistory> GetChatHistoryByXrefIDAndSubjectIDAsync(Guid clientRelationXrefID, Logic.Objects.Client subjectClient)
        {
            try
            {
                MessageHistory logicHistory = new MessageHistory();
                ClientRelationXref contextRelationship = await santaContext.ClientRelationXref
                    .Include(r => r.EventType)
                        .ThenInclude(e => e.ClientRelationXref)
                    .Include(r => r.EventType)
                        .ThenInclude(e => e.Survey)
                    .Include(r => r.SenderClient)
                    .Include(r => r.RecipientClient)
                    .Include(r => r.ChatMessage)
                        .ThenInclude(cm => cm.MessageReceiverClient)
                    .Include(r => r.ChatMessage)
                        .ThenInclude(cm => cm.MessageSenderClient)
                    .FirstOrDefaultAsync(x => x.ClientRelationXrefId == clientRelationXrefID);

                logicHistory.relationXrefID = clientRelationXrefID;
                logicHistory.eventType = Mapper.MapEvent(contextRelationship.EventType);


                logicHistory.subjectClient = Mapper.MapClientMeta(subjectClient);
                logicHistory.subjectMessages = contextRelationship.ChatMessage
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => m.senderClient.clientId == subjectClient.clientID)
                    .ToList();
                foreach(Message logicMessage in logicHistory.subjectMessages)
                {
                    logicMessage.subjectMessage = true;
                }

                logicHistory.recieverMessages = contextRelationship.ChatMessage
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => m.senderClient.clientId != subjectClient.clientID)
                    .ToList();

                logicHistory.assignmentRecieverClient = Mapper.MapClientMeta(contextRelationship.RecipientClient);
                logicHistory.assignmentSenderClient = Mapper.MapClientMeta(contextRelationship.SenderClient);
                logicHistory.conversationClient = Mapper.MapClientMeta(contextRelationship.SenderClient);


                return logicHistory;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<MessageHistory> GetGeneralChatHistoryBySubjectIDAsync(Logic.Objects.Client conversationClient, Logic.Objects.Client subjectClient)
        {
            try
            {
                MessageHistory logicHistory = new MessageHistory();
                List<Entities.ChatMessage> contextListMessages = await santaContext.ChatMessage
                    .Where(m => m.ClientRelationXrefId == null && (m.MessageSenderClientId == conversationClient.clientID || m.MessageReceiverClientId == conversationClient.clientID))
                    .Include(s => s.MessageSenderClient)
                    .Include(r => r.MessageReceiverClient)
                    .OrderBy(dt => dt.DateTimeSent)
                    .ToListAsync();

                // General histories dont have a relationXrefID, EventType, or AssignmentClient because they are not tied to an assignment
                logicHistory.relationXrefID = null;
                logicHistory.eventType = new Event();
                logicHistory.assignmentRecieverClient = new ClientMeta();
                logicHistory.assignmentSenderClient = new ClientMeta();
                logicHistory.conversationClient = Mapper.MapClientMeta(conversationClient);

                logicHistory.subjectClient = Mapper.MapClientMeta(subjectClient);
                logicHistory.subjectMessages = contextListMessages
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => m.senderClient.clientId == subjectClient.clientID)
                    .ToList();
                foreach (Message logicMessage in logicHistory.subjectMessages)
                {
                    logicMessage.subjectMessage = true;
                }

                logicHistory.recieverMessages = contextListMessages
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => m.senderClient.clientId != subjectClient.clientID)
                    .ToList();

                return logicHistory;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion

        #endregion

        #region ClientStatus
        public async Task<List<Status>> GetAllClientStatus()
        {
            try
            {
                List<Logic.Objects.Status> logicStatusList = (await santaContext.ClientStatus.ToListAsync()).Select(Mapper.MapStatus).ToList();
                return logicStatusList;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<Status> GetClientStatusByID(Guid clientStatusID)
        {
            try
            {
                Logic.Objects.Status logicStatus = Mapper.MapStatus(await santaContext.ClientStatus
                    .FirstOrDefaultAsync(s => s.ClientStatusId == clientStatusID));
                return logicStatus;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task CreateStatusAsync(Status newStatus)
        {
            try
            {
                Data.Entities.ClientStatus contextStatus = Mapper.MapStatus(newStatus);
                await santaContext.ClientStatus.AddAsync(contextStatus);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task UpdateStatusByIDAsync(Status changedLogicStatus)
        {
            try
            {
                ClientStatus targetStatus = await santaContext.ClientStatus.FirstOrDefaultAsync(s => s.ClientStatusId == changedLogicStatus.statusID);
                if (targetStatus == null)
                {
                    throw new Exception("Client Status was not found. Update failed for status");
                }
                targetStatus.StatusDescription = changedLogicStatus.statusDescription;
                santaContext.ClientStatus.Update(targetStatus);

            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task DeleteStatusByIDAsync(Guid clientStatusID)
        {
            try
            {
                ClientStatus contextStatus = await santaContext.ClientStatus.FirstOrDefaultAsync(s => s.ClientStatusId == clientStatusID);
                santaContext.ClientStatus.Remove(contextStatus);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion

        #region Event
        public async Task CreateEventAsync(Event newEvent)
        {
            try
            {
                Data.Entities.EventType contextEvent = Mapper.MapEvent(newEvent);
                await santaContext.EventType.AddAsync(contextEvent);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task DeleteEventByIDAsync(Guid eventID)
        {
            try
            {
                Data.Entities.EventType contextEvent = await santaContext.EventType.FirstOrDefaultAsync(e => e.EventTypeId == eventID);
                santaContext.EventType.Remove(contextEvent);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<List<Event>> GetAllEvents()
        {
            try
            {
                List<Logic.Objects.Event> eventList = (await santaContext.EventType
                    .Include(e => e.ClientRelationXref)
                    .Include(e => e.Survey)
                    .ToListAsync())
                    .Select(Mapper.MapEvent)
                    .ToList();
                return eventList;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<Event> GetEventByIDAsync(Guid eventID)
        {
            try
            {
                Logic.Objects.Event logicEvent = Mapper.MapEvent(await santaContext.EventType
                    .Include(e => e.ClientRelationXref)
                    .Include(e => e.Survey)
                    .FirstOrDefaultAsync(e => e.EventTypeId == eventID));
                return logicEvent;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<Event> GetEventByNameAsync(string eventName)
        {
            try
            {
                Logic.Objects.Event logicEvent = Mapper.MapEvent(await santaContext.EventType
                    .Include(e => e.Survey)
                    .Include(e => e.ClientRelationXref)
                    .FirstOrDefaultAsync(e => e.EventDescription == eventName));
                return logicEvent;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task UpdateEventByIDAsync(Event targetEvent)
        {
            try
            {
                Data.Entities.EventType oldContextEvent = await santaContext.EventType.FirstOrDefaultAsync(e => e.EventTypeId == targetEvent.eventTypeID);

                oldContextEvent.EventDescription = targetEvent.eventDescription;
                oldContextEvent.IsActive = targetEvent.active;

                santaContext.Update(oldContextEvent);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion

        #region Survey
        public async Task CreateSurveyAsync(Logic.Objects.Survey newSurvey)
        {
            try
            {
                Data.Entities.Survey contextSurvey = Mapper.MapSurvey(newSurvey);
                await santaContext.Survey.AddAsync(contextSurvey);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<List<Logic.Objects.Survey>> GetAllSurveys()
        {
            try
            {
                List<Logic.Objects.Survey> surveyList = (await santaContext.Survey
                    .Include(s => s.SurveyQuestionXref)
                        .ThenInclude(sqx => sqx.SurveyQuestion)
                            .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                                .ThenInclude(so => so.SurveyOption)
                    .ToListAsync())

                    .Select(Mapper.MapSurvey).ToList();

                return surveyList;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<Logic.Objects.Survey> GetSurveyByID(Guid surveyId)
        {
            try
            {
                Logic.Objects.Survey logicSurvey = Mapper.MapSurvey(await santaContext.Survey
                    .Include(s => s.SurveyQuestionXref)
                        .ThenInclude(sqx => sqx.SurveyQuestion)
                            .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                                .ThenInclude(sqox => sqox.SurveyOption)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.SurveyId == surveyId));
                return logicSurvey;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task UpdateSurveyByIDAsync(Logic.Objects.Survey targetSurvey)
        {
            try
            {
                Data.Entities.Survey contextOldSurvey = await santaContext.Survey.FirstOrDefaultAsync(s => s.SurveyId == targetSurvey.surveyID);
                contextOldSurvey.SurveyDescription = targetSurvey.surveyDescription;
                contextOldSurvey.IsActive = targetSurvey.active;
                santaContext.Update(contextOldSurvey);

            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task DeleteSurveyByIDAsync(Guid surveyID)
        {
            try
            {
                Data.Entities.Survey contextSurvey = await santaContext.Survey.FirstOrDefaultAsync(s => s.SurveyId == surveyID);
                santaContext.Survey.Remove(contextSurvey);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task DeleteSurveyQuestionXrefBySurveyIDAndQuestionID(Guid surveyID, Guid surveyQuestionID)
        {
            try
            {
                Data.Entities.SurveyQuestionXref contextSurveyQuestionXref = await santaContext.SurveyQuestionXref.FirstOrDefaultAsync(sqx => sqx.SurveyId == surveyID && sqx.SurveyQuestionId == surveyQuestionID);
                santaContext.SurveyQuestionXref.Remove(contextSurveyQuestionXref);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion

        #region SurveyOption

        public async Task<List<Option>> GetAllSurveyOption()
        {
            try
            {
                List<Option> listLogicSurveyOption = (await santaContext.SurveyOption
                    .Include(s => s.SurveyQuestionOptionXref)
                    .Include(s => s.SurveyResponse)
                    .ToListAsync())
                    .Select(Mapper.MapSurveyOption)
                    .ToList();
                return listLogicSurveyOption;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<Option> GetSurveyOptionByIDAsync(Guid surveyOptionID)
        {
            try
            {
                Option logicOption = Mapper.MapSurveyOption(await santaContext.SurveyOption
                    .Include(s => s.SurveyResponse)
                    .Include(s => s.SurveyQuestionOptionXref)
                    .FirstOrDefaultAsync(so => so.SurveyOptionId == surveyOptionID));
                return logicOption;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        /// <summary>
        /// Creates a SurveyOption and adds it to the context
        /// </summary>
        /// <param name="newSurveyOption"></param>
        /// <returns></returns>
        public async Task CreateSurveyOptionAsync(Option newSurveyOption)
        {
            try
            {
                Data.Entities.SurveyOption contextQuestionOption = Mapper.MapSurveyOption(newSurveyOption);
                await santaContext.SurveyOption.AddAsync(contextQuestionOption);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        
        /// <summary>
        /// Updates a survey option by its ID and queues that update in the context.
        /// </summary>
        /// <param name="targetSurveyOption"></param>
        /// <returns></returns>
        public async Task UpdateSurveyOptionByIDAsync(Option targetSurveyOption)
        {
            try
            {
                Data.Entities.SurveyOption oldOption = await santaContext.SurveyOption.FirstOrDefaultAsync(o => o.SurveyOptionId == targetSurveyOption.surveyOptionID);

                oldOption.DisplayText = targetSurveyOption.displayText;
                oldOption.SurveyOptionValue = targetSurveyOption.surveyOptionValue;

                santaContext.SurveyOption.Update(oldOption);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        
        /// <summary>
        /// Dletes a survey option by ID and queues the update in the context.
        /// </summary>
        /// <param name="surveyOptionID"></param>
        /// <returns></returns>
        public async Task DeleteSurveyOptionByIDAsync(Guid surveyOptionID)
        {
            try
            {
                santaContext.SurveyOption.Remove(await santaContext.SurveyOption.FirstOrDefaultAsync(o => o.SurveyOptionId == surveyOptionID));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion

        #region SurveyQuestionOptionXref
        public async Task CreateSurveyQuestionOptionXrefAsync(Option newQuestionOption, Guid surveyQuestionID, bool isActive, string sortOrder)
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
            try
            {
                List<Question> listLogicQuestion = (await santaContext.SurveyQuestion
                    .Include(sq => sq.SurveyResponse)
                    .Include(sq => sq.SurveyQuestionOptionXref)
                        .ThenInclude(so => so.SurveyOption)
                    .ToListAsync())

                    .Select(Mapper.MapQuestion).ToList();
                return listLogicQuestion;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<Question> GetSurveyQuestionByIDAsync(Guid questionID)
        {
            try
            {
                Logic.Objects.Question logicQuestion = Mapper.MapQuestion(await santaContext.SurveyQuestion
                    .Include(sq => sq.SurveyResponse)
                    .Include(sq => sq.SurveyQuestionOptionXref)
                        .ThenInclude(so => so.SurveyOption)
                    .FirstOrDefaultAsync(q => q.SurveyQuestionId == questionID));
                return logicQuestion;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task CreateSurveyQuestionAsync(Question newQuestion)
        {
            try
            {
                Entities.SurveyQuestion contextQuestion = Mapper.MapQuestion(newQuestion);
                await santaContext.SurveyQuestion.AddAsync(contextQuestion);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

        }
        public async Task CreateSurveyQuestionXrefAsync(Guid surveyID, Guid questionID)
        {
            try
            {
                Data.Entities.SurveyQuestionXref contextQuestionXref = new SurveyQuestionXref()
                {
                    SurveyQuestionXrefId = Guid.NewGuid(),
                    SurveyQuestionId = questionID,
                    SurveyId = surveyID,
                    SortOrder = "asc",
                    IsActive = true
                };
                await santaContext.SurveyQuestionXref.AddAsync(contextQuestionXref);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

        }
        public async Task UpdateSurveyQuestionByIDAsync(Question targetQuestion)
        {
            try
            {
                Data.Entities.SurveyQuestion oldQuestion = await santaContext.SurveyQuestion.FirstOrDefaultAsync(q => q.SurveyQuestionId == targetQuestion.questionID);

                oldQuestion.QuestionText = targetQuestion.questionText;
                oldQuestion.IsSurveyOptionList = targetQuestion.isSurveyOptionList;
                oldQuestion.SenderCanView = targetQuestion.senderCanView;

                santaContext.SurveyQuestion.Update(oldQuestion);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task DeleteSurveyQuestionByIDAsync(Guid surveyQuestionID)
        {
            try
            {
                santaContext.SurveyQuestion.Remove(await santaContext.SurveyQuestion.FirstOrDefaultAsync(q => q.SurveyQuestionId == surveyQuestionID));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion

        #region Response

        public async Task<Logic.Objects.Response> GetSurveyResponseByIDAsync(Guid surveyResponseID)
        {
            try
            {
                Logic.Objects.Response logicResponse = Mapper.MapResponse(await santaContext.SurveyResponse
                    .Include(sr => sr.Survey)
                        .ThenInclude(s => s.EventType)
                    .Include(sr => sr.SurveyQuestion)
                        .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                            .ThenInclude(sqox => sqox.SurveyOption)
                    .FirstOrDefaultAsync(r => r.SurveyResponseId == surveyResponseID));
                return logicResponse;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<List<Logic.Objects.Response>> GetAllSurveyResponsesByClientID(Guid clientID)
        {
            try
            {
                List<Response> listLogicResponse = (await santaContext.SurveyResponse
                    .Include(sr => sr.Survey)
                        .ThenInclude(s => s.EventType)
                    .Include(sr => sr.SurveyQuestion)
                        .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                            .ThenInclude(sqox => sqox.SurveyOption)
                    .Where(r => r.ClientId == clientID)
                    .ToListAsync())
                    .Select(Mapper.MapResponse)
                    .ToList();
                return listLogicResponse;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<List<Logic.Objects.Response>> GetAllSurveyResponses()
        {
            try
            {
                List<Response> listLogicResponse = (await santaContext.SurveyResponse
                    .Include(sr => sr.Survey)
                        .ThenInclude(s => s.EventType)
                    .Include(sr => sr.SurveyQuestion)
                        .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                            .ThenInclude(sqox => sqox.SurveyOption)
                    .ToListAsync())
                    .Select(Mapper.MapResponse)
                    .ToList();
                return listLogicResponse;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task CreateSurveyResponseAsync(Response newResponse)
        {
            try
            {
                Data.Entities.SurveyResponse contextResponse = Mapper.MapResponse(newResponse);
                await santaContext.SurveyResponse.AddAsync(contextResponse);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task DeleteSurveyResponseByIDAsync(Guid surveyResponseID)
        {
            try
            {
                Data.Entities.SurveyResponse contextResponse = await santaContext.SurveyResponse.FirstOrDefaultAsync(r => r.SurveyResponseId == surveyResponseID);
                santaContext.Remove(contextResponse);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task UpdateSurveyResponseByIDAsync(Response targetResponse)
        {
            try
            {
                Data.Entities.SurveyResponse contextOldResponse = await santaContext.SurveyResponse.FirstOrDefaultAsync(r => r.SurveyResponseId == targetResponse.surveyResponseID);

                contextOldResponse.ResponseText = targetResponse.responseText;

                santaContext.SurveyResponse.Update(contextOldResponse);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion

        #region Board Entry
        public async Task CreateBoardEntryAsync(Logic.Objects.BoardEntry newEntry)
        {
            try
            {
                await santaContext.AddAsync(Mapper.MapBoardEntry(newEntry));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<List<Logic.Objects.BoardEntry>> GetAllBoardEntriesAsync()
        {
            try
            {
                List<Logic.Objects.BoardEntry> logicListBoardEntries = (await santaContext.BoardEntry
                    .ToListAsync())
                    .Select(Mapper.MapBoardEntry)
                    .ToList();
                return logicListBoardEntries;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<Logic.Objects.BoardEntry> GetBoardEntryByIDAsync(Guid boardEntryID)
        {
            try
            {
                Logic.Objects.BoardEntry logicBoardEntry = Mapper.MapBoardEntry(await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.BoardEntryId == boardEntryID));
                return logicBoardEntry;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<Logic.Objects.BoardEntry> GetBoardEntryByPostNumberAsync(int postNumber)
        {
            try
            {
                Logic.Objects.BoardEntry logicBoardEntry = Mapper.MapBoardEntry(await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.PostNumber == postNumber));
                return logicBoardEntry;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task UpdateBoardEntryPostNumberAsync(Logic.Objects.BoardEntry newEntry)
        {
            try
            {
                Data.Entities.BoardEntry contextBoardEntry = await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.BoardEntryId == newEntry.boardEntryID);

                contextBoardEntry.PostNumber = newEntry.postNumber;

                santaContext.BoardEntry.Update(contextBoardEntry);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task UpdateBoardEntryPostDescriptionAsync(Logic.Objects.BoardEntry newEntry)
        {
            try
            {
                Data.Entities.BoardEntry contextBoardEntry = await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.BoardEntryId == newEntry.boardEntryID);

                contextBoardEntry.PostDescription = newEntry.postDescription;

                santaContext.BoardEntry.Update(contextBoardEntry);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task UpdateBoardEntryTypeAsync(Logic.Objects.BoardEntry newEntry)
        {
            try
            {
                Data.Entities.BoardEntry contextBoardEntry = await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.BoardEntryId == newEntry.boardEntryID);

                contextBoardEntry.EntryTypeId = newEntry.entryType.entryTypeID;

                santaContext.BoardEntry.Update(contextBoardEntry);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task DeleteBoardEntryByIDAsync(Guid boardEntryID)
        {
            try
            {
                Entities.BoardEntry contextBoardEntry = await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.BoardEntryId == boardEntryID);
                santaContext.Remove(contextBoardEntry);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        #endregion

        #region Entry Type
        public async Task CreateEntryTypeAsync(Logic.Objects.EntryType newEntryType)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<List<Logic.Objects.EntryType>> GetAllEntryTypesAsync()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<Logic.Objects.EntryType> GetEntryTypeByIDAsync(Guid entryTypeID)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task UpdateEntryTypeName(Logic.Objects.EntryType updatedEntryType)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task UpdateEntryTypeDescription(Logic.Objects.EntryType updatedEntryType)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task DeleteEntryTypeByID(Guid entryTypeID)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion

        #region Utility
        public async Task SaveAsync()
        {
            try
            {
                await santaContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

        }
        #endregion
    }
}
