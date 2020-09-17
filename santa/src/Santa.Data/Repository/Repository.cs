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
        public async Task CreateClientRelationByID(Guid senderClientID, Guid recipientClientID, Guid eventTypeID, Guid assignmentStatusID)
        {
            try
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
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task<List<Logic.Objects.Client>> GetAllClients()
        {
            try
            {
                List<Logic.Objects.Client> clientList = (await santaContext.Client
                    /* Surveys and responses */
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.SurveyQuestion)
                            .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                                .ThenInclude(sqox => sqox.SurveyOption)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.Survey)
                            .ThenInclude(s => s.EventType)

                    /* Sender/Assignment info and Tags */
                    .Include(c => c.ClientRelationXrefRecipientClient)
                        .ThenInclude(crxsc => crxsc.SenderClient)
                            .ThenInclude(c => c.ClientTagXref)
                                .ThenInclude(txr => txr.Tag)
                    .Include(c => c.ClientRelationXrefRecipientClient)
                        .ThenInclude(crxrc => crxrc.RecipientClient)
                            .ThenInclude(c => c.ClientTagXref)
                                .ThenInclude(txr => txr.Tag)
                    .Include(c => c.ClientRelationXrefSenderClient)
                        .ThenInclude(crxsc => crxsc.SenderClient)
                            .ThenInclude(c => c.ClientTagXref)
                                .ThenInclude(txr => txr.Tag)
                    .Include(c => c.ClientRelationXrefSenderClient)
                        .ThenInclude(crxrc => crxrc.RecipientClient)
                            .ThenInclude(c => c.ClientTagXref)
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
                    .ToListAsync())
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
                    /* Surveys and responses */
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.SurveyQuestion)
                            .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                                .ThenInclude(sqox => sqox.SurveyOption)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.Survey)
                            .ThenInclude(s => s.EventType)

                    /* Sender/Assignment info and Tags */
                    .Include(c => c.ClientRelationXrefRecipientClient)
                        .ThenInclude(crxsc => crxsc.SenderClient)
                            .ThenInclude(c => c.ClientTagXref)
                                .ThenInclude(txr => txr.Tag)
                    .Include(c => c.ClientRelationXrefRecipientClient)
                        .ThenInclude(crxrc => crxrc.RecipientClient)
                            .ThenInclude(c => c.ClientTagXref)
                                .ThenInclude(txr => txr.Tag)
                    .Include(c => c.ClientRelationXrefSenderClient)
                        .ThenInclude(crxsc => crxsc.SenderClient)
                            .ThenInclude(c => c.ClientTagXref)
                                .ThenInclude(txr => txr.Tag)
                    .Include(c => c.ClientRelationXrefSenderClient)
                        .ThenInclude(crxrc => crxrc.RecipientClient)
                            .ThenInclude(c => c.ClientTagXref)
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
                    /* Surveys and responses */
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.SurveyQuestion)
                            .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                                .ThenInclude(sqox => sqox.SurveyOption)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.Survey)
                            .ThenInclude(s => s.EventType)

                    /* Sender/Assignment info and Tags */
                    .Include(c => c.ClientRelationXrefRecipientClient)
                        .ThenInclude(crxsc => crxsc.SenderClient)
                            .ThenInclude(c => c.ClientTagXref)
                                .ThenInclude(txr => txr.Tag)
                    .Include(c => c.ClientRelationXrefRecipientClient)
                        .ThenInclude(crxrc => crxrc.RecipientClient)
                            .ThenInclude(c => c.ClientTagXref)
                                .ThenInclude(txr => txr.Tag)
                    .Include(c => c.ClientRelationXrefSenderClient)
                        .ThenInclude(crxsc => crxsc.SenderClient)
                            .ThenInclude(c => c.ClientTagXref)
                                .ThenInclude(txr => txr.Tag)
                    .Include(c => c.ClientRelationXrefSenderClient)
                        .ThenInclude(crxrc => crxrc.RecipientClient)
                            .ThenInclude(c => c.ClientTagXref)
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
                contextOldClient.HasAccount = targetLogicClient.hasAccount;

                santaContext.Client.Update(contextOldClient);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public async Task UpdateAssignmentProgressStatusByID(Guid assignmentID, Guid newAssignmentStatusID)
        {
            try
            {
                ClientRelationXref contextRelationship = await santaContext.ClientRelationXref.FirstOrDefaultAsync(crxf => crxf.ClientRelationXrefId == assignmentID);

                contextRelationship.AssignmentStatusId = newAssignmentStatusID;

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

        #region Assignment Status
        public async Task CreateAssignmentStatus(Logic.Objects.AssignmentStatus newAssignmentStatus)
        {
            try
            {
                Data.Entities.AssignmentStatus newContextAssignmentStatus = Mapper.MapAssignmentStatus(newAssignmentStatus);
                await santaContext.AssignmentStatus.AddAsync(newContextAssignmentStatus);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<List<Logic.Objects.AssignmentStatus>> GetAllAssignmentStatuses()
        {
            try
            {
                List<Logic.Objects.AssignmentStatus> listLogicAssigmentStatus = (await santaContext.AssignmentStatus.ToListAsync()).Select(Mapper.MapAssignmentStatus).ToList();

                return listLogicAssigmentStatus;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<Logic.Objects.AssignmentStatus> GetAssignmentStatusByID(Guid assignmentStatusID)
        {
            try
            {
                Logic.Objects.AssignmentStatus logicAssignmentStatus = Mapper.MapAssignmentStatus(await santaContext.AssignmentStatus.FirstOrDefaultAsync(stat => stat.AssignmentStatusId == assignmentStatusID));

                return logicAssignmentStatus;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<List<object>> GetAssignmentsByAssignmentStatusID(Guid assignmentStatusID)
        {
            try
            {
                //var assignments = (await santaContext.ClientRelationXref.Select(crxr => crxr.AssignmentStatusId == assignmentStatusID).ToListAsync()).Select(Mapper.MapRelationRecipientXref)
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task UpdateAssignmentStatus(Logic.Objects.AssignmentStatus targetAssignmentStatus)
        {
            try
            {
                Data.Entities.AssignmentStatus contextAssignmentStatus = await santaContext.AssignmentStatus.FirstOrDefaultAsync(stat => stat.AssignmentStatusId == targetAssignmentStatus.assignmentStatusID);

                contextAssignmentStatus.AssignmentStatusName = targetAssignmentStatus.assignmentStatusName;
                contextAssignmentStatus.AssignmentStatusDescription = targetAssignmentStatus.assignmentStatusDescription;

                santaContext.AssignmentStatus.Update(contextAssignmentStatus);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task DeleteAssignmentStatusByID(Guid assignmentStatusID)
        {
            try
            {
                Entities.AssignmentStatus contextAssignmentStatus = await santaContext.AssignmentStatus.FirstOrDefaultAsync(stat => stat.AssignmentStatusId == assignmentStatusID);

                santaContext.Remove(contextAssignmentStatus);
            }
            catch (Exception e)
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
                    /* Assignment information and surveys */
                    .Include(r => r.ClientRelationXrefSenderClient)
                        .ThenInclude(clXref => clXref.RecipientClient)
                            .ThenInclude(c => c.SurveyResponse.Where(r => r.SurveyQuestion.SenderCanView == true))
                                .ThenInclude(sr => sr.Survey)
                                    .ThenInclude(s => s.EventType)
                    .Include(r => r.ClientRelationXrefSenderClient)
                        .ThenInclude(clXref => clXref.RecipientClient)
                            .ThenInclude(c => c.SurveyResponse)
                                .ThenInclude(sr => sr.SurveyQuestion)

                    /* Assignment event types */
                    .Include(r => r.ClientRelationXrefSenderClient)
                        .ThenInclude(e => e.EventType)

                    /* Assignment Statuses */
                    .Include(r => r.ClientRelationXrefRecipientClient)
                    .Include(xr => xr.ClientRelationXrefRecipientClient)
                        .ThenInclude(m => m.AssignmentStatus)
                    .Include(xr => xr.ClientRelationXrefSenderClient)
                        .ThenInclude(m => m.AssignmentStatus)

                    /* Sender/Assignment Tags */
                    .Include(c => c.ClientRelationXrefSenderClient)
                        .ThenInclude(crxsc => crxsc.SenderClient)
                            .ThenInclude(c => c.ClientTagXref)
                                .ThenInclude(txr => txr.Tag)
                    .Include(c => c.ClientRelationXrefRecipientClient)
                        .ThenInclude(crxrc => crxrc.RecipientClient)
                            .ThenInclude(c => c.ClientTagXref)
                                .ThenInclude(txr => txr.Tag)

                    /* Profile approval status */
                    .Include(s => s.ClientStatus)

                    /* Profile survey responses aand event types */
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(s => s.SurveyQuestion)
                    .Include(c => c.SurveyResponse)
                        .ThenInclude(sr => sr.Survey)
                            .ThenInclude(s => s.EventType)

                    .FirstOrDefaultAsync(c => c.Email == email));

                List<Response> responsesToRemove = new List<Response>();
                // For each recipient in the profile
                foreach (ProfileRecipient recipient in logicProfile.assignments)
                {
                    // And for each response that recipient gave
                    foreach(Response response in recipient.responses)
                    {
                        // If the recipients event does not match the responses event
                        if(recipient.recipientEvent.eventTypeID != response.responseEvent.eventTypeID)
                        {
                            // Add it to the list of responses to remove from thsi assignment
                            responsesToRemove.Add(response);
                        }
                    }
                    // Then once that is done, remove all the responses from that recipient that were gathered, and reset the list for any others
                    foreach(Response response in responsesToRemove)
                    {
                        recipient.responses.Remove(response);
                    }
                    responsesToRemove.Clear();
                }
                
                
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
                List<Entities.Client> listContextClient = await santaContext.Client
                    .Include(c => c.ClientStatus)
                    .Include(c => c.ClientRelationXrefRecipientClient)
                    .Include(c => c.ClientRelationXrefRecipientClient)
                        .ThenInclude(x => x.AssignmentStatus)
                    .ToListAsync();

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
                    .Include(r => r.AssignmentStatus)
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

                logicHistory.assignmentStatus = Mapper.MapAssignmentStatus(contextRelationship.AssignmentStatus);


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
                logicHistory.assignmentStatus = new Logic.Objects.AssignmentStatus();
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
                    .Include(b => b.EntryType)
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
                Logic.Objects.BoardEntry logicBoardEntry = Mapper.MapBoardEntry(await santaContext.BoardEntry
                    .Include(b => b.EntryType)
                    .FirstOrDefaultAsync(b => b.BoardEntryId == boardEntryID));
                return logicBoardEntry;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<Logic.Objects.BoardEntry> GetBoardEntryByThreadAndPostNumberAsync(int threadNumber, int postNumber)
        {
            try
            {
                Logic.Objects.BoardEntry logicBoardEntry = Mapper.MapBoardEntry(await santaContext.BoardEntry
                    .Include(b => b.EntryType)
                    .FirstOrDefaultAsync(b => b.ThreadNumber == threadNumber && b.PostNumber == postNumber));
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
        public async Task UpdateBoardEntryThreadNumberAsync(Logic.Objects.BoardEntry newEntry)
        {
            try
            {
                Data.Entities.BoardEntry contextBoardEntry = await santaContext.BoardEntry.FirstOrDefaultAsync(b => b.BoardEntryId == newEntry.boardEntryID);

                contextBoardEntry.ThreadNumber = newEntry.threadNumber;

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
                await santaContext.EntryType.AddAsync(Mapper.MapEntryType(newEntryType));
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
                List<Logic.Objects.EntryType> listLogicEntryType = (await santaContext.EntryType
                    .ToListAsync())
                    .Select(Mapper.MapEntryType)
                    .ToList();
                return listLogicEntryType;
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
                Logic.Objects.EntryType logicEntryType = Mapper.MapEntryType(await santaContext.EntryType.FirstOrDefaultAsync(e => e.EntryTypeId == entryTypeID));
                return logicEntryType;
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
                Entities.EntryType contextEntryType = await santaContext.EntryType.FirstOrDefaultAsync(e => e.EntryTypeId == updatedEntryType.entryTypeID);

                contextEntryType.EntryTypeName = updatedEntryType.entryTypeName;

                santaContext.EntryType.Update(contextEntryType);
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
                Entities.EntryType contextEntryType = await santaContext.EntryType.FirstOrDefaultAsync(e => e.EntryTypeId == updatedEntryType.entryTypeID);

                contextEntryType.EntryTypeDescription = updatedEntryType.entryTypeDescription;

                santaContext.EntryType.Update(contextEntryType);
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
                Entities.EntryType contextEntryType = await santaContext.EntryType.FirstOrDefaultAsync(e => e.EntryTypeId == entryTypeID);

                santaContext.EntryType.Remove(contextEntryType);
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
        public async Task<List<ClientMeta>> GetAllAllowedAssignmentsByID(Guid clientID, Guid eventTypeID)
        {
            try
            {
                Logic.Objects.Client logicClient = await GetClientByIDAsync(clientID);
                List<Logic.Objects.Client> allClients = await GetAllClients();
                List<ClientMeta> allowedAssignments = new List<ClientMeta>();


                foreach(Logic.Objects.Client potentialAssignment in allClients)
                {
                    // If the client doesnt have any assignments that match the potential assignment and eventType, the potential assignment is approved, and the potential assignment is not the current client
                    if(!logicClient.assignments.Any<RelationshipMeta>(c => c.relationshipClient.clientId == potentialAssignment.clientID && c.eventType.eventTypeID == eventTypeID) && potentialAssignment.clientStatus.statusDescription == Constants.APPROVED_STATUS && potentialAssignment.clientID != clientID)
                    {
                        allowedAssignments.Add(Mapper.MapClientMeta(potentialAssignment));
                    }
                }
                return allowedAssignments;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion
    }
}
