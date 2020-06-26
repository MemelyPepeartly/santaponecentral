using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using Santa.Data.Entities;
using Santa.Data.Repository;

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
        /// <summary>
        /// Creates a new client and adds it to the context
        /// </summary>
        /// <param name="newClient"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Creates relation between a two clients by an event. A sender and a reciever
        /// </summary>
        /// <param name="senderClientID"></param>
        /// <param name="recipientClientID"></param>
        /// <param name="eventTypeID"></param>
        /// <returns></returns>
        public async Task CreateClientRelationByID(Guid senderClientID, Guid recipientClientID, Guid eventTypeID)
        {
            try
            {
                Data.Entities.ClientRelationXref contexRelation = new ClientRelationXref()
                {
                    ClientRelationXrefId = Guid.NewGuid(),
                    SenderClientId = senderClientID,
                    RecipientClientId = recipientClientID,
                    EventTypeId = eventTypeID
                };
                await santaContext.ClientRelationXref.AddAsync(contexRelation);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        
        /// <summary>
        /// Gets a list of all clients
        /// </summary>
        /// <returns></returns>
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
                    .Include(tx => tx.ClientTagXref)
                        .ThenInclude(t => t.Tag)
                    .Include(s => s.ClientStatus).ToListAsync())

                    .Select(Mapper.MapClient).ToList();
                return clientList;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        /// <summary>
        /// Gets a client by their ID
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<Logic.Objects.Client> GetClientByIDAsync(Guid clientId)
        {
            try
            {
                Logic.Objects.Client logicClient = Mapper.MapClient(await santaContext.Client
                    .Include(s => s.ClientRelationXrefSenderClient)
                        .ThenInclude(u => u.SenderClient)
                    .Include(r => r.ClientRelationXrefRecipientClient)
                        .ThenInclude(u => u.RecipientClient)
                    .Include(tx => tx.ClientTagXref)
                        .ThenInclude(t => t.Tag)
                    .Include(s => s.ClientStatus)
                    .FirstOrDefaultAsync(c => c.ClientId == clientId));
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

                santaContext.Client.Update(contextOldClient);
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
                        .ThenInclude(u => u.RecipientClient) 
                        .ThenInclude(recRes => recRes.SurveyResponse)
                    .Include(r => r.ClientRelationXrefSenderClient)
                        .ThenInclude(e => e.EventType)
                    .Include(res => res.SurveyResponse)
                    .Include(s => s.ClientStatus)
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
        /// <summary>
        /// Creates a new tag
        /// </summary>
        /// <param name="newTag"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Creates a new relationship between a client and a tag
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="tagID"></param>
        /// <returns></returns>
        public async Task CreateClientTagRelationByID(Guid clientID, Guid tagID)
        {
            try
            {
                Data.Entities.ClientTagXref contextClientRelationXref = new ClientTagXref()
                {
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
        /// <summary>
        /// Gets a tag by its ID
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
        public async Task<Logic.Objects.Tag> GetTagByIDAsync(Guid tagID)
        {
            try
            {
                Logic.Objects.Tag logicTag = Mapper.MapTag(await santaContext.Tag.FirstOrDefaultAsync(t => t.TagId == tagID));
                return logicTag;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        /// <summary>
        /// Gets a list of all tags
        /// </summary>
        /// <returns></returns>
        public async Task<List<Logic.Objects.Tag>> GetAllTags()
        {
            try
            {
                List<Logic.Objects.Tag> logicTags = (await santaContext.Tag.ToListAsync()).Select(Mapper.MapTag).ToList();

                return logicTags;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        /// <summary>
        /// Updates a tag's name by its ID
        /// </summary>
        /// <param name="targetLogicTag"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Deletes a client tag relationship
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="tagID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Deletes a tag
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
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
                    .Include(r => r.MessageRecieverClient)
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
                Logic.Objects.Message logicMessage = Mapper.MapMessage(await santaContext.ChatMessage.FirstOrDefaultAsync(m => m.ChatMessageId == chatMessageID));
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
        /// <summary>
        /// Gets all the chat histories for every client relationship and includes general chats with null XrefID.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MessageHistory>> GetAllChatHistories()
        {
            try
            {
                List<MessageHistory> listLogicMessageHistory = new List<MessageHistory>();
                List<ClientRelationXref> listContextClientRelationXref = await santaContext.ClientRelationXref.ToListAsync();
                List<Entities.Client> listContextClient = await santaContext.Client.ToListAsync();

                MessageHistory logicHistory = new MessageHistory();
                foreach (Entities.Client client in listContextClient)
                {
                    foreach (ClientRelationXref relationship in listContextClientRelationXref)
                    {
                        logicHistory = new MessageHistory();
                        logicHistory = await GetChatHistoryByClientIDAndOptionalRelationXrefIDAsync(client.ClientId, relationship.ClientRelationXrefId);
                        listLogicMessageHistory.Add(logicHistory);
                    }
                    logicHistory = new MessageHistory();
                    logicHistory = await GetChatHistoryByClientIDAndOptionalRelationXrefIDAsync(client.ClientId, null);
                }
                return listLogicMessageHistory;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        /// <summary>
        /// Gets all the chat histories that have unread messages
        /// </summary>
        /// <returns></returns>
        public Task<List<MessageHistory>> GetAllChatHistoriesWithUnreadMessagesAsync()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets all the chat histories of clients by their ID. Does not include general chats with null relationship Xref ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public async Task<List<MessageHistory>> GetAllChatHistoriesByClientIDAsync(Guid clientID)
        {
            try
            {
                List<MessageHistory> listLogicMessageHistory = new List<MessageHistory>();
                List<ClientRelationXref> XrefList = await santaContext.ClientRelationXref.Where(x => x.SenderClientId == clientID).ToListAsync();


                foreach (ClientRelationXref relationship in XrefList)
                {
                    MessageHistory logicHistory = new MessageHistory();
                    logicHistory = await GetChatHistoryByClientIDAndOptionalRelationXrefIDAsync(clientID, relationship.ClientRelationXrefId);
                    listLogicMessageHistory.Add(logicHistory);
                }

                return listLogicMessageHistory;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        /// <summary>
        /// Gets a chat history by eventID. Does not include general chats, as those are not tied to any event
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="clientRelationXrefID"></param>
        /// <returns></returns>
        public async Task<List<MessageHistory>> GetAllChatHistoriesByEventIDAsync(Guid eventID)
        {
            try
            {
                List<MessageHistory> listLogicHistories = new List<MessageHistory>();
                MessageHistory logicHistory = new MessageHistory();
                List<ClientRelationXref> XrefList = await santaContext.ClientRelationXref.Where(x => x.EventTypeId == eventID).ToListAsync();

                foreach(ClientRelationXref relationship in XrefList)
                {
                    List<Message> logicListMessages = (await santaContext.ChatMessage
                    .Where(m => m.ClientRelationXrefId == relationship.ClientRelationXrefId)
                    .Include(s => s.MessageSenderClient)
                    .Include(r => r.MessageRecieverClient)
                    .Include(x => x.ClientRelationXref)
                    .OrderBy(dt => dt.DateTimeSent)
                    .ToListAsync())
                    .Select(Mapper.MapMessage)
                    .ToList();

                    logicHistory.history = logicListMessages;
                    logicHistory.relationXrefID = relationship.ClientRelationXrefId;

                    logicHistory.eventType = await FindEventByXrefID(relationship.ClientRelationXrefId);
                    logicHistory.eventSenderClient = await FindSenderClientMetaByXrefID(relationship.ClientRelationXrefId);
                    logicHistory.eventRecieverClient = await FindRecieverClientMetaByXrefID(relationship.ClientRelationXrefId);

                    listLogicHistories.Add(logicHistory);
                    logicHistory = new MessageHistory();
                }

                return listLogicHistories;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        /// <summary>
        /// Gets a single chat history by clientID and XrefID. This is a relationship between a person sending a gift that was assigned. Xref ID is optional in this case to be able to pull general chats without a relationship attatched.
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="clientRelationXrefID"></param>
        /// <returns></returns>
        public async Task<MessageHistory> GetChatHistoryByClientIDAndOptionalRelationXrefIDAsync(Guid clientID, Guid? clientRelationXrefID)
        {
            try
            {
                MessageHistory logicHistory = new MessageHistory();
                List<Message> logicListMessages = (await santaContext.ChatMessage
                    .Where(m => m.ClientRelationXrefId == clientRelationXrefID && (m.MessageSenderClientId == clientID || m.MessageRecieverClientId == clientID))
                    .Include(s => s.MessageSenderClient)
                    .Include(r => r.MessageRecieverClient)
                    .Include(x => x.ClientRelationXref)
                    .OrderBy(dt => dt.DateTimeSent)
                    .ToListAsync())
                    .Select(Mapper.MapMessage)
                    .ToList();
                logicHistory.history = logicListMessages;
                logicHistory.relationXrefID = clientRelationXrefID;

                logicHistory.eventType = clientRelationXrefID != null ? await FindEventByXrefID(clientRelationXrefID) : new Event();
                logicHistory.eventSenderClient = clientRelationXrefID != null ? await FindSenderClientMetaByXrefID(clientRelationXrefID) : new MessageClientMeta();
                logicHistory.eventRecieverClient = clientRelationXrefID != null ? await FindRecieverClientMetaByXrefID(clientRelationXrefID) : new MessageClientMeta();

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
        /// <summary>
        /// Gets a list of all statuses
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Gets a status by clientStatusID
        /// </summary>
        /// <param name="clientStatusID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Creates a new status
        /// </summary>
        /// <param name="newStatus"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Updates a status description that exists in the database
        /// </summary>
        /// <param name="changedLogicStatus"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Deletes a status by a given ID
        /// </summary>
        /// <param name="clientStatusID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Creates a new event type in the database
        /// </summary>
        /// <param name="newEvent"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Deletes an event by eventID 
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets a list of all events
        /// </summary>
        /// <returns></returns>
        public async Task<List<Event>> GetAllEvents()
        {
            try
            {
                List<Logic.Objects.Event> eventList = (await santaContext.EventType.ToListAsync()).Select(Mapper.MapEvent).ToList();
                return eventList;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        /// <summary>
        /// Gets an event by a given ID
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
        public async Task<Event> GetEventByIDAsync(Guid eventID)
        {
            try
            {
                Logic.Objects.Event logicEvent = Mapper.MapEvent(await santaContext.EventType.FirstOrDefaultAsync(e => e.EventTypeId == eventID));
                return logicEvent;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        /// <summary>
        /// Updates either an eventDescripton or active state that exists in database
        /// </summary>
        /// <param name="targetEvent"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Creates a survey and adds it to the context
        /// </summary>
        /// <param name="newSurvey"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets all surveys
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Gets a survey by its ID
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Updates a survey with targetSurvey information
        /// </summary>
        /// <param name="targetSurvey"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Deletes a survey by its ID
        /// </summary>
        /// <param name="surveyID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Deletes a relationship between a survey and the question in it, taking it off of the survey, but not deleting the question.
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="surveyQuestionID"></param>
        /// <returns></returns>
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
                List<Option> listLogicSurveyOption = (await santaContext.SurveyOption.Include(s => s.SurveyQuestionOptionXref).ToListAsync()).Select(Mapper.MapSurveyOption).ToList();
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
                Option logicOption = Mapper.MapSurveyOption(await santaContext.SurveyOption.Include(s => s.SurveyQuestionOptionXref).FirstOrDefaultAsync(so => so.SurveyOptionId == surveyOptionID));
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
        /// <summary>
        /// Creates the Xref between a Survey Question and a Survey Option
        /// </summary>
        /// <param name="newQuestionOption"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets a list of all surveyQuestions
        /// </summary>
        /// <returns></returns>
        public async Task<List<Question>> GetAllSurveyQuestions()
        {
            try
            {
                List<Question> listLogicQuestion = (await santaContext.SurveyQuestion
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

        /// <summary>
        /// Creates a survey question and adds it to the context
        /// </summary>
        /// <param name="newQuestion"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Creates the cross reference between a survey and the questions it has and adds it to the context
        /// </summary>
        /// <param name="logicQuestion"></param>
        /// <returns></returns>
        public async Task CreateSurveyQuestionXrefAsync(Question logicQuestion)
        {
            try
            {
                Data.Entities.SurveyQuestionXref contextQuestionXref = Mapper.MapQuestionXref(logicQuestion);
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
        public async Task<Logic.Objects.Response> GetSurveyResponseByIDAsync(Guid surveyResponseID)
        {
            try
            {
                Logic.Objects.Response logicResponse = Mapper.MapResponse(await santaContext.SurveyResponse.FirstOrDefaultAsync(r => r.SurveyResponseId == surveyResponseID));
                return logicResponse;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<List<Logic.Objects.Response>> GetAllSurveyResponsesByClientID(Guid clientID)
        {
            try
            {
                List<Response> listLogicResponse = (await santaContext.SurveyResponse.Where(r => r.ClientId == clientID).ToListAsync()).Select(Mapper.MapResponse).ToList();
                return listLogicResponse;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<List<Logic.Objects.Response>> GetAllSurveyResponses()
        {
            try
            {
                List<Response> listLogicResponse = (await santaContext.SurveyResponse.ToListAsync()).Select(Mapper.MapResponse).ToList();
                return listLogicResponse;
            }
            catch (Exception e)
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
        
        #region Utility
        /// <summary>
        /// Saves changes made to the context
        /// </summary>
        /// <returns></returns>
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
        public async Task<Event> FindEventByXrefID(Guid? clientRelationXrefID)
        {
            try
            {
                ClientRelationXref contextXrefRelationship = await santaContext.ClientRelationXref
                    .Include(e => e.EventType)
                    .FirstOrDefaultAsync(x => x.ClientRelationXrefId == clientRelationXrefID);

                Event logicEvent = Mapper.MapEvent(contextXrefRelationship.EventType);
                return logicEvent;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<MessageClientMeta> FindRecieverClientMetaByXrefID(Guid? clientRelationXrefID)
        {
            try
            {
                ClientRelationXref contextXrefRelationship = await santaContext.ClientRelationXref
                    .Include(r => r.RecipientClient)
                    .FirstOrDefaultAsync(x => x.ClientRelationXrefId == clientRelationXrefID);

                MessageClientMeta logicMeta = Mapper.MapClientMeta(contextXrefRelationship.RecipientClient);
                return logicMeta;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<MessageClientMeta> FindSenderClientMetaByXrefID(Guid? clientRelationXrefID)
        {
            try
            {
                ClientRelationXref contextXrefRelationship = await santaContext.ClientRelationXref
                    .Include(r => r.SenderClient)
                    .FirstOrDefaultAsync(x => x.ClientRelationXrefId == clientRelationXrefID);

                MessageClientMeta logicMeta = Mapper.MapClientMeta(contextXrefRelationship.SenderClient);
                return logicMeta;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion
    }
}
