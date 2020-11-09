using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Santa.Logic.Interfaces;
using Santa.Logic.Objects;
using Santa.Data.Entities;
using Santa.Logic.Constants;
using Santa.Logic.Objects.Information_Objects;
using Santa.Logic.Objects.Base_Objects.Logging;
using YuleLog = Santa.Logic.Objects.Base_Objects.Logging.YuleLog;
using Category = Santa.Logic.Objects.Base_Objects.Logging.Category;
using System.Reflection.Metadata.Ecma335;

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
        public async Task<List<StrippedClient>> GetAllStrippedClientData()
        {
            List<StrippedClient> clientList = await santaContext.Client
                .Select(client => new StrippedClient()
                {
                    clientID = client.ClientId,
                    email = client.Email,
                    nickname = client.Nickname,
                    clientName = client.ClientName,
                    isAdmin = client.IsAdmin,
                    clientStatus = Mapper.MapStatus(client.ClientStatus),
                    responses = client.SurveyResponse.Select(surveyResponse => new Response()
                    {
                        surveyResponseID = surveyResponse.SurveyResponseId,
                        clientID = surveyResponse.ClientId,
                        surveyID = surveyResponse.SurveyId,
                        surveyOptionID = surveyResponse.SurveyOptionId,
                        responseText = surveyResponse.ResponseText,
                        responseEvent = Mapper.MapEvent(surveyResponse.Survey.EventType),
                        surveyQuestion = Mapper.MapQuestion(surveyResponse.SurveyQuestion)
                    }).ToList(),
                    tags = client.ClientTagXref.Select(tagXref => new Logic.Objects.Tag()
                    {
                        tagID = tagXref.TagId,
                        tagName = tagXref.Tag.TagName,
                    }).ToList()

                }).AsNoTracking().ToListAsync();
            return clientList;
        }
        public async Task<List<BaseClient>> GetAllBasicClientInformation()
        {
            List<BaseClient> clientList = await santaContext.Client.Select(client => new BaseClient()
            {
                clientID = client.ClientId,
                clientName = client.ClientName,
                nickname = client.Nickname,
                email = client.Email,
                isAdmin = client.IsAdmin,
                hasAccount = client.HasAccount,

            }).ToListAsync();
            return clientList;
        }
        public async Task<List<Logic.Objects.Client>> GetAllClients()
        {
            List<Logic.Objects.Client> clientList = await santaContext.Client
                .Select(client => new Logic.Objects.Client()
                {
                    clientID = client.ClientId,
                    email = client.Email,
                    nickname = client.Nickname,
                    clientName = client.ClientName,
                    isAdmin = client.IsAdmin,
                    hasAccount = client.HasAccount,
                    clientStatus = Mapper.MapStatus(client.ClientStatus),
                    address = new Address
                    {
                        addressLineOne = client.AddressLine1,
                        addressLineTwo = client.AddressLine2,
                        city = client.City,
                        country = client.Country,
                        state = client.State,
                        postalCode = client.PostalCode
                    },
                    responses = client.SurveyResponse.Select(surveyResponse => new Response()
                    {
                        surveyResponseID = surveyResponse.SurveyResponseId,
                        clientID = surveyResponse.ClientId,
                        surveyID = surveyResponse.SurveyId,
                        surveyOptionID = surveyResponse.SurveyOptionId,
                        responseText = surveyResponse.ResponseText,
                        responseEvent = Mapper.MapEvent(surveyResponse.Survey.EventType),
                        surveyQuestion = Mapper.MapQuestion(surveyResponse.SurveyQuestion)
                    }).ToList(),
                    notes = client.Note.Select(note => new Logic.Objects.Base_Objects.Note()
                    {
                        noteID = note.NoteId,
                        noteSubject = note.NoteSubject,
                        noteContents = note.NoteContents
                    }).ToList(),
                    tags = client.ClientTagXref.Select(tagXref => new Logic.Objects.Tag()
                    {
                        tagID = tagXref.TagId,
                        tagName = tagXref.Tag.TagName,
                    }).ToList(),
                    assignments = client.ClientRelationXrefSenderClient.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = xref.SenderClientId == client.ClientId ? Mapper.MapClientMeta(xref.RecipientClient) : Mapper.MapClientMeta(xref.SenderClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>()
                        /*
                        tags = xref.SenderClientId == client.ClientId ? xref.RecipientClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList() :
                        xref.SenderClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList(),
                        
                        removable = xref.ChatMessage.Count > 0 ? false : true
                        */
                    }).ToList(),
                    senders = client.ClientRelationXrefRecipientClient.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = xref.SenderClientId == client.ClientId ? Mapper.MapClientMeta(xref.RecipientClient) : Mapper.MapClientMeta(xref.SenderClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>()
                        /*
                        tags = xref.SenderClientId == client.ClientId ? xref.RecipientClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList() :
                        xref.SenderClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList(),
                        
                        removable = xref.ChatMessage.Count > 0 ? false : true
                        */
                    }).ToList()

                }).AsNoTracking().ToListAsync();

            return clientList;
        }
        public async Task<Logic.Objects.Client> GetClientByIDAsync(Guid clientId)
        {
            Logic.Objects.Client logicClient = await santaContext.Client
                .Select(client => new Logic.Objects.Client()
                {
                    clientID = client.ClientId,
                    email = client.Email,
                    nickname = client.Nickname,
                    clientName = client.ClientName,
                    isAdmin = client.IsAdmin,
                    hasAccount = client.HasAccount,
                    clientStatus = Mapper.MapStatus(client.ClientStatus),
                    address = new Address
                    {
                        addressLineOne = client.AddressLine1,
                        addressLineTwo = client.AddressLine2,
                        city = client.City,
                        country = client.Country,
                        state = client.State,
                        postalCode = client.PostalCode
                    },
                    responses = client.SurveyResponse.Select(surveyResponse => new Response()
                    {
                        surveyResponseID = surveyResponse.SurveyResponseId,
                        clientID = surveyResponse.ClientId,
                        surveyID = surveyResponse.SurveyId,
                        surveyOptionID = surveyResponse.SurveyOptionId,
                        responseText = surveyResponse.ResponseText,
                        responseEvent = Mapper.MapEvent(surveyResponse.Survey.EventType),
                        surveyQuestion = Mapper.MapQuestion(surveyResponse.SurveyQuestion)
                    }).ToList(),
                    notes = client.Note.Select(note => new Logic.Objects.Base_Objects.Note()
                    {
                        noteID = note.NoteId,
                        noteSubject = note.NoteSubject,
                        noteContents = note.NoteContents
                    }).ToList(),
                    tags = client.ClientTagXref.Select(tagXref => new Logic.Objects.Tag()
                    {
                        tagID = tagXref.TagId,
                        tagName = tagXref.Tag.TagName,
                    }).ToList(),
                    assignments = client.ClientRelationXrefSenderClient.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = xref.SenderClientId == client.ClientId ? Mapper.MapClientMeta(xref.RecipientClient) : Mapper.MapClientMeta(xref.SenderClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>()
                        /*
                        tags = xref.SenderClientId == client.ClientId ? xref.RecipientClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList() :
                        xref.SenderClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList(),
                        
                        removable = xref.ChatMessage.Count > 0 ? false : true
                        */
                    }).ToList(),
                    senders = client.ClientRelationXrefRecipientClient.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = xref.SenderClientId == client.ClientId ? Mapper.MapClientMeta(xref.RecipientClient) : Mapper.MapClientMeta(xref.SenderClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>()
                        /*
                        tags = xref.SenderClientId == client.ClientId ? xref.RecipientClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList() :
                        xref.SenderClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList(),
                        
                        removable = xref.ChatMessage.Count > 0 ? false : true
                        */
                    }).ToList()

                }).AsNoTracking().FirstOrDefaultAsync(c => c.clientID == clientId);

            return logicClient;
        }
        public async Task<Logic.Objects.Client> GetClientByEmailAsync(string clientEmail)
        {
            Logic.Objects.Client logicClient = await santaContext.Client
                .Select(client => new Logic.Objects.Client()
                {
                    clientID = client.ClientId,
                    email = client.Email,
                    nickname = client.Nickname,
                    clientName = client.ClientName,
                    isAdmin = client.IsAdmin,
                    hasAccount = client.HasAccount,
                    clientStatus = Mapper.MapStatus(client.ClientStatus),
                    address = new Address
                    {
                        addressLineOne = client.AddressLine1,
                        addressLineTwo = client.AddressLine2,
                        city = client.City,
                        country = client.Country,
                        state = client.State,
                        postalCode = client.PostalCode
                    },
                    responses = client.SurveyResponse.Select(surveyResponse => new Response()
                    {
                        surveyResponseID = surveyResponse.SurveyResponseId,
                        clientID = surveyResponse.ClientId,
                        surveyID = surveyResponse.SurveyId,
                        surveyOptionID = surveyResponse.SurveyOptionId,
                        responseText = surveyResponse.ResponseText,
                        responseEvent = Mapper.MapEvent(surveyResponse.Survey.EventType),
                        surveyQuestion = Mapper.MapQuestion(surveyResponse.SurveyQuestion)
                    }).ToList(),
                    notes = client.Note.Select(note => new Logic.Objects.Base_Objects.Note()
                    {
                        noteID = note.NoteId,
                        noteSubject = note.NoteSubject,
                        noteContents = note.NoteContents
                    }).ToList(),
                    tags = client.ClientTagXref.Select(tagXref => new Logic.Objects.Tag()
                    {
                        tagID = tagXref.TagId,
                        tagName = tagXref.Tag.TagName,
                    }).ToList(),
                    assignments = client.ClientRelationXrefSenderClient.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = xref.SenderClientId == client.ClientId ? Mapper.MapClientMeta(xref.RecipientClient) : Mapper.MapClientMeta(xref.SenderClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>()
                        /*
                        tags = xref.SenderClientId == client.ClientId ? xref.RecipientClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList() :
                        xref.SenderClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList(),
                        
                        removable = xref.ChatMessage.Count > 0 ? false : true
                        */
                    }).ToList(),
                    senders = client.ClientRelationXrefRecipientClient.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = xref.SenderClientId == client.ClientId ? Mapper.MapClientMeta(xref.RecipientClient) : Mapper.MapClientMeta(xref.SenderClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>()
                        /*
                        tags = xref.SenderClientId == client.ClientId ? xref.RecipientClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList() :
                        xref.SenderClient.ClientTagXref.ToList().Select(tagXref => new Logic.Objects.Tag()
                        {
                            tagID = tagXref.TagId,
                            tagName = tagXref.Tag.TagName
                        }).OrderBy(t => t.tagName).ToList(),
                        
                        removable = xref.ChatMessage.Count > 0 ? false : true
                        */
                    }).ToList()
                }).AsNoTracking().FirstOrDefaultAsync(c => c.email == clientEmail);

            return logicClient;
        }
        public async Task<Logic.Objects.Client> GetStaticClientObjectByID(Guid clientID)
        {
            return Mapper.MapStaticClient((await santaContext.Client.AsNoTracking().FirstAsync(c => c.ClientId == clientID)));
        }
        public async Task<Logic.Objects.Client> GetStaticClientObjectByEmail(string email)
        {
            return Mapper.MapStaticClient((await santaContext.Client.AsNoTracking().FirstAsync(c => c.Email == email)));
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

                /* Profile survey responses and event types */
                .Include(c => c.SurveyResponse)
                    .ThenInclude(s => s.SurveyQuestion.SurveyQuestionXref)
                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.Survey.EventType)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == email));

            return logicProfile;
        }
        public async Task<Logic.Objects.Profile> GetProfileByIDAsync(Guid clientID)
        {
            Logic.Objects.Profile logicProfile = Mapper.MapProfile(await santaContext.Client
                /* Profile survey responses aand event types */
                .Include(c => c.SurveyResponse)
                    .ThenInclude(s => s.SurveyQuestion.SurveyQuestionXref)
                .Include(c => c.SurveyResponse)
                    .ThenInclude(sr => sr.Survey.EventType)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ClientId == clientID));

            return logicProfile;
        }
        public async Task<List<ProfileAssignment>> GetProfileAssignments(Guid clientID)
        {
            List<ProfileAssignment> listLogicProfileAssignment = await santaContext.ClientRelationXref.Where(xr => xr.SenderClientId == clientID)
                .Select(xref => new ProfileAssignment()
                {
                    relationXrefID = xref.ClientRelationXrefId,
                    recipientClient = Mapper.MapClientMeta(xref.RecipientClient),
                    recipientEvent = Mapper.MapEvent(xref.EventType),
                    assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),

                    address = new Address
                    {
                        addressLineOne = xref.RecipientClient.AddressLine1,
                        addressLineTwo = xref.RecipientClient.AddressLine2,
                        city = xref.RecipientClient.City,
                        country = xref.RecipientClient.Country,
                        state = xref.RecipientClient.State,
                        postalCode = xref.RecipientClient.PostalCode
                    },
                    responses = xref.RecipientClient.SurveyResponse.Where(r => r.SurveyQuestion.SenderCanView == true).Select(surveyResponse => new Response()
                    {
                        surveyResponseID = surveyResponse.SurveyResponseId,
                        clientID = surveyResponse.ClientId,
                        surveyID = surveyResponse.SurveyId,
                        surveyOptionID = surveyResponse.SurveyOptionId,
                        responseText = surveyResponse.ResponseText,
                        responseEvent = Mapper.MapEvent(surveyResponse.Survey.EventType),
                        surveyQuestion = Mapper.MapQuestion(surveyResponse.SurveyQuestion)
                    }).ToList()
                }).ToListAsync();
            return listLogicProfileAssignment;
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
                listLogicMessageHistory.Add(Mapper.MapHistoryInformation(contextRelationship, subjectClient, false));
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

            return Mapper.MapHistoryInformation(contextRelationship, subjectClient, false);
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
        public async Task<List<MessageHistory>> GetUnloadedProfileChatHistoriesAsync(Guid profileOwnerClientID)
        {
            Logic.Objects.Client logicSubject = await GetStaticClientObjectByID(profileOwnerClientID);
            List<MessageHistory> listLogicMessageHistory = await santaContext.ClientRelationXref.Where(r => r.SenderClientId == profileOwnerClientID)
                .Select(xref => new MessageHistory()
                {
                    relationXrefID = xref.ClientRelationXrefId,
                    eventType = Mapper.MapEvent(xref.EventType),
                    assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),

                    subjectClient = Mapper.MapClientChatMeta(logicSubject),
                    conversationClient = Mapper.MapClientChatMeta(xref.SenderClient),
                    assignmentRecieverClient = Mapper.MapClientChatMeta(xref.RecipientClient),
                    assignmentSenderClient = Mapper.MapClientChatMeta(xref.SenderClient),

                    subjectMessages = new List<Message>(),
                    recieverMessages = new List<Message>(),

                    unreadCount = xref.ChatMessage.Where(m => m.FromAdmin && m.IsMessageRead == false).Count()
                }).ToListAsync();


            return listLogicMessageHistory;
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

        #region Note
        public async Task CreateNoteAsync(Logic.Objects.Base_Objects.Note newNote, Guid clientID)
        {
            Note contextNote = Mapper.MapNote(newNote);
            contextNote.ClientId = clientID;
            await santaContext.Note.AddAsync(contextNote);
        }

        public async Task<List<Logic.Objects.Base_Objects.Note>> GetAllNotesAsync()
        {
            return (await santaContext.Note.ToListAsync())
                .Select(Mapper.MapNote)
                .ToList();
        }

        public async Task<Logic.Objects.Base_Objects.Note> GetNoteByIDAsync(Guid noteID)
        {
            return Mapper.MapNote(await santaContext.Note.FirstOrDefaultAsync(n => n.NoteId == noteID));
        }

        public async Task UpdateNote(Logic.Objects.Base_Objects.Note updatedNote)
        {
            Note contextNote = await santaContext.Note.FirstOrDefaultAsync(n => n.NoteId == updatedNote.noteID);

            contextNote.NoteSubject = updatedNote.noteSubject;
            contextNote.NoteContents = updatedNote.noteContents;

            santaContext.Note.Update(contextNote);
        }

        public async Task DeleteNoteByID(Guid noteID)
        {
            Note contextNote = await santaContext.Note.FirstOrDefaultAsync(n => n.NoteId == noteID);

            santaContext.Note.Remove(contextNote);
        }
        #endregion

        #region Category
        public async Task CreateNewCategory(Category newCategory)
        {
            Data.Entities.Category contextCategory = Mapper.MapCategory(newCategory);
            await santaContext.Category.AddAsync(contextCategory);
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return (await santaContext.Category.ToListAsync()).Select(Mapper.MapCategory).ToList();
        }

        public async Task<Category> GetCategoryByID(Guid categoryID)
        {
            Category logicCategory = Mapper.MapCategory(await santaContext.Category.FirstOrDefaultAsync(c => c.CategoryId == categoryID));
            return logicCategory;
        }

        public async Task UpdateCategory(Category targetCategory)
        {
            Entities.Category contextCategory = await santaContext.Category.FirstOrDefaultAsync(c => c.CategoryId == targetCategory.categoryID);
            contextCategory.CategoryName = targetCategory.categoryName;
            contextCategory.CategoryDescription = targetCategory.categoryDescription;

            santaContext.Category.Update(contextCategory);
        }

        public async Task DeleteCategoryByID(Guid categoryID)
        {
            Entities.Category contextCategory = await santaContext.Category.FirstOrDefaultAsync(c => c.CategoryId == categoryID);
            santaContext.Category.Remove(contextCategory);
        }
        #endregion

        #region Yule Log
        public async Task CreateNewLogEntry(YuleLog newLog)
        {
            Data.Entities.YuleLog contextLog = Mapper.MapLog(newLog);
            await santaContext.YuleLog.AddAsync(contextLog);
        }

        public async Task<List<YuleLog>> GetAllLogEntries()
        {
            List<YuleLog> logicLogList = (await santaContext.YuleLog
                .Include(yl => yl.Category)
                .ToListAsync())
                .Select(Mapper.MapLog)
                .ToList();
            return logicLogList;
        }

        public async Task<YuleLog> GetLogByID(Guid logID)
        {
            YuleLog logicLog = Mapper.MapLog(await santaContext.YuleLog.FirstOrDefaultAsync(yl => yl.LogId == logID));
            return logicLog;
        }

        public async Task<List<YuleLog>> GetLogsByCategoryID(Guid categoryID)
        {
            List<YuleLog> logicLogList = (await santaContext.YuleLog
                .Include(yl => yl.Category)
                .Where(yl => yl.CategoryId == categoryID)
                .ToListAsync())
                .Select(Mapper.MapLog)
                .ToList();
            return logicLogList;
        }
        #endregion

        #region Informational
        public async Task<List<RelationshipMeta>> getClientAssignmentsByIDAsync(Guid clientID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RelationshipMeta>> getClientSendersByIDAsync(Guid clientID)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Utility
        public async Task SaveAsync()
        {
            await santaContext.SaveChangesAsync();
        }
        public async Task<List<AllowedAssignmentMeta>> GetAllAllowedAssignmentsByID(Guid clientID, Guid eventTypeID)
        {
            List<Logic.Objects.Client> allClients = await GetAllClients();
            Logic.Objects.Client logicClient = allClients.FirstOrDefault(c => c.clientID == clientID);
            List<AllowedAssignmentMeta> allowedAssignments = new List<AllowedAssignmentMeta>();


            foreach (Logic.Objects.Client potentialAssignment in allClients)
            {
                // If the client doesnt have any assignments that match the potential assignment and eventType, the potential assignment is approved, and the potential assignment is not the current client
                if (!logicClient.assignments.Any<RelationshipMeta>(c => c.relationshipClient.clientId == potentialAssignment.clientID && c.eventType.eventTypeID == eventTypeID) && potentialAssignment.clientStatus.statusDescription == Constants.APPROVED_STATUS && potentialAssignment.clientID != clientID)
                {
                    AllowedAssignmentMeta assignment = Mapper.MapAllowedAssignmentMeta(potentialAssignment);
                    assignment.answeredSurveys = new List<Logic.Objects.Information_Objects.SurveyMeta>();
                    // For each response in the potential assignments response list
                    foreach (Response response in potentialAssignment.responses)
                    {
                        // If the assignment object does not have any survey values where the surveyID and EventID are already in the list
                        if(!assignment.answeredSurveys.Any(s => s.surveyID == response.surveyID && s.eventTypeID == response.responseEvent.eventTypeID))
                        {
                            // Add it to the list of answered surveys
                            assignment.answeredSurveys.Add(Mapper.MapSurveyMeta(response));
                        }
                    }
                    allowedAssignments.Add(assignment);
                }
            }
            return allowedAssignments;
        }

        public async Task<List<StrippedClient>> SearchClientByQuery(SearchQueries searchQuery)
        {
            List<StrippedClient> allClientList = await GetAllStrippedClientData();
            List<StrippedClient> matchingClients = new List<StrippedClient>();

            if (searchQuery.isHardSearch)
            {
                matchingClients = allClientList
                    .Where(c => !searchQuery.tags.Any() || searchQuery.tags.All(queryTagID => c.tags.Any(clientTag => clientTag.tagID == queryTagID)))
                    .Where(c => !searchQuery.statuses.Any() || searchQuery.statuses.All(queryStatusID => c.clientStatus.statusID == queryStatusID))
                    .Where(c => !searchQuery.events.Any() || searchQuery.events.All(queryEventID => c.responses.Any(r => r.responseEvent.eventTypeID == queryEventID)))
                    .Where(c => !searchQuery.names.Any() || searchQuery.names.All(queryName => c.clientName == queryName))
                    .Where(c => !searchQuery.nicknames.Any() || searchQuery.nicknames.All(queryNickname => c.nickname == queryNickname)).ToList()
                    .Where(c => !searchQuery.emails.Any() || searchQuery.emails.All(queryEmail => c.email == queryEmail)).ToList();
            }
            else
            {
                matchingClients = allClientList
                    .Where(c => !searchQuery.tags.Any() || searchQuery.tags.Any(queryTagID => c.tags.Any(t => t.tagID == queryTagID)))
                    .Where(c => !searchQuery.statuses.Any() || searchQuery.statuses.Any(queryStatusID => c.clientStatus.statusID == queryStatusID))
                    .Where(c => !searchQuery.events.Any() || searchQuery.events.Any(queryEventID => c.responses.Any(r => r.responseEvent.eventTypeID == queryEventID)))
                    .Where(c => !searchQuery.names.Any() || searchQuery.names.Any(queryName => c.clientName == queryName))
                    .Where(c => !searchQuery.nicknames.Any() || searchQuery.nicknames.Any(queryNickname => c.nickname == queryNickname)).ToList()
                    .Where(c => !searchQuery.emails.Any() || searchQuery.emails.Any(queryEmail => c.email == queryEmail)).ToList();
            }

            return matchingClients;
        }
        #endregion
    }
}
