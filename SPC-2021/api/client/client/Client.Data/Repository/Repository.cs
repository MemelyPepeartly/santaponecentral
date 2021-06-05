using Client.Data.Entities;
using Client.Logic.Constants;
using Client.Logic.Interfaces;
using Client.Logic.Objects;
using Client.Logic.Objects.Information_Objects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Data.Repository
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
            await santaContext.Clients.AddAsync(contextClient);
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
            await santaContext.ClientRelationXrefs.AddAsync(contexRelation);
        }
        public async Task<Logic.Objects.Client> GetClientByIDAsync(Guid clientId)
        {
            Logic.Objects.Client logicClient = await santaContext.Clients
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
                    responses = client.SurveyResponses.Select(surveyResponse => new Response()
                    {
                        surveyResponseID = surveyResponse.SurveyResponseId,
                        clientID = surveyResponse.ClientId,
                        surveyID = surveyResponse.SurveyId,
                        surveyOptionID = surveyResponse.SurveyOptionId,
                        responseText = surveyResponse.ResponseText,
                        responseEvent = Mapper.MapEvent(surveyResponse.Survey.EventType),
                        surveyQuestion = Mapper.MapQuestion(surveyResponse.SurveyQuestion)
                    }).ToList(),
                    notes = client.Notes.Select(note => new Logic.Objects.Base_Objects.Note()
                    {
                        noteID = note.NoteId,
                        noteSubject = note.NoteSubject,
                        noteContents = note.NoteContents
                    }).ToList(),
                    tags = client.ClientTagXrefs.Select(tagXref => new Logic.Objects.Tag()
                    {
                        tagID = tagXref.TagId,
                        tagName = tagXref.Tag.TagName,
                    }).ToList(),
                    assignments = client.ClientRelationXrefSenderClients.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = xref.SenderClientId == client.ClientId ? Mapper.MapClientMeta(xref.RecipientClient) : Mapper.MapClientMeta(xref.SenderClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>(),
                        removable = xref.ChatMessages.Count > 0 ? false : true
                    }).ToList(),
                    senders = client.ClientRelationXrefRecipientClients.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = xref.SenderClientId == client.ClientId ? Mapper.MapClientMeta(xref.RecipientClient) : Mapper.MapClientMeta(xref.SenderClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>(),
                        removable = xref.ChatMessages.Count > 0 ? false : true
                    }).ToList()

                }).AsNoTracking().FirstOrDefaultAsync(c => c.clientID == clientId);

            return logicClient;
        }
        public async Task<Logic.Objects.Client> GetClientByEmailAsync(string clientEmail)
        {
            Logic.Objects.Client logicClient = await santaContext.Clients
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
                    responses = client.SurveyResponses.Select(surveyResponse => new Response()
                    {
                        surveyResponseID = surveyResponse.SurveyResponseId,
                        clientID = surveyResponse.ClientId,
                        surveyID = surveyResponse.SurveyId,
                        surveyOptionID = surveyResponse.SurveyOptionId,
                        responseText = surveyResponse.ResponseText,
                        responseEvent = Mapper.MapEvent(surveyResponse.Survey.EventType),
                        surveyQuestion = Mapper.MapQuestion(surveyResponse.SurveyQuestion)
                    }).ToList(),
                    notes = client.Notes.Select(note => new Logic.Objects.Base_Objects.Note()
                    {
                        noteID = note.NoteId,
                        noteSubject = note.NoteSubject,
                        noteContents = note.NoteContents
                    }).ToList(),
                    tags = client.ClientTagXrefs.Select(tagXref => new Logic.Objects.Tag()
                    {
                        tagID = tagXref.TagId,
                        tagName = tagXref.Tag.TagName,
                    }).ToList(),
                    assignments = client.ClientRelationXrefSenderClients.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = xref.SenderClientId == client.ClientId ? Mapper.MapClientMeta(xref.RecipientClient) : Mapper.MapClientMeta(xref.SenderClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>()
                    }).ToList(),
                    senders = client.ClientRelationXrefRecipientClients.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = xref.SenderClientId == client.ClientId ? Mapper.MapClientMeta(xref.RecipientClient) : Mapper.MapClientMeta(xref.SenderClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>()
                    }).ToList()
                }).AsNoTracking().FirstOrDefaultAsync(c => c.email == clientEmail);

            return logicClient;
        }
        public async Task<Logic.Objects.Client> GetStaticClientObjectByID(Guid clientID)
        {
            return Mapper.MapStaticClient((await santaContext.Clients.AsNoTracking().FirstAsync(c => c.ClientId == clientID)));
        }
        public async Task<Logic.Objects.Client> GetStaticClientObjectByEmail(string email)
        {
            return Mapper.MapStaticClient((await santaContext.Clients.AsNoTracking().FirstAsync(c => c.Email == email)));
        }

        #region Minimized Client Data Getters
        public async Task<List<StrippedClient>> GetAllStrippedClientData()
        {
            List<StrippedClient> clientList = await santaContext.Clients
                .Select(client => new StrippedClient()
                {
                    clientID = client.ClientId,
                    email = client.Email,
                    nickname = client.Nickname,
                    clientName = client.ClientName,
                    isAdmin = client.IsAdmin,
                    clientStatus = Mapper.MapStatus(client.ClientStatus),
                    responses = client.SurveyResponses.Select(surveyResponse => new Response()
                    {
                        surveyResponseID = surveyResponse.SurveyResponseId,
                        clientID = surveyResponse.ClientId,
                        surveyID = surveyResponse.SurveyId,
                        surveyOptionID = surveyResponse.SurveyOptionId,
                        responseText = surveyResponse.ResponseText,
                        responseEvent = Mapper.MapEvent(surveyResponse.Survey.EventType),
                        surveyQuestion = Mapper.MapQuestion(surveyResponse.SurveyQuestion)
                    }).ToList(),
                    tags = client.ClientTagXrefs.Select(tagXref => new Logic.Objects.Tag()
                    {
                        tagID = tagXref.TagId,
                        tagName = tagXref.Tag.TagName,
                    }).ToList(),
                    giftAssignments = client.ClientRelationXrefSenderClients.Where(x => x.EventType.EventDescription == Constants.GIFT_EXCHANGE_EVENT).Count(),
                    giftSenders = client.ClientRelationXrefRecipientClients.Where(x => x.EventType.EventDescription == Constants.GIFT_EXCHANGE_EVENT).Count(),
                    cardAssignments = client.ClientRelationXrefSenderClients.Where(x => x.EventType.EventDescription == Constants.CARD_EXCHANGE_EVENT).Count(),
                    cardSenders = client.ClientRelationXrefRecipientClients.Where(x => x.EventType.EventDescription == Constants.CARD_EXCHANGE_EVENT).Count()

                }).AsNoTracking().ToListAsync();
            return clientList;
        }
        public async Task<StrippedClient> GetStrippedClientDataByID(Guid clientID)
        {
            StrippedClient logicStrippedClient = await santaContext.Clients
                .Select(client => new StrippedClient()
                {
                    clientID = client.ClientId,
                    email = client.Email,
                    nickname = client.Nickname,
                    clientName = client.ClientName,
                    isAdmin = client.IsAdmin,
                    clientStatus = Mapper.MapStatus(client.ClientStatus),
                    responses = client.SurveyResponses.Select(surveyResponse => new Response()
                    {
                        surveyResponseID = surveyResponse.SurveyResponseId,
                        clientID = surveyResponse.ClientId,
                        surveyID = surveyResponse.SurveyId,
                        surveyOptionID = surveyResponse.SurveyOptionId,
                        responseText = surveyResponse.ResponseText,
                        responseEvent = Mapper.MapEvent(surveyResponse.Survey.EventType),
                        surveyQuestion = Mapper.MapQuestion(surveyResponse.SurveyQuestion)
                    }).ToList(),
                    tags = client.ClientTagXrefs.Select(tagXref => new Logic.Objects.Tag()
                    {
                        tagID = tagXref.TagId,
                        tagName = tagXref.Tag.TagName,
                    }).ToList()

                }).AsNoTracking().FirstOrDefaultAsync(c => c.clientID == clientID);

            return logicStrippedClient;
        }
        public async Task<List<BaseClient>> GetAllBasicClientInformation()
        {
            List<BaseClient> clientList = await santaContext.Clients.Select(client => new BaseClient()
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
        public async Task<List<HQClient>> GetAllHeadquarterClients()
        {
            List<HQClient> clientList = await santaContext.Clients
                .Select(client => new HQClient()
                {
                    clientID = client.ClientId,
                    email = client.Email,
                    nickname = client.Nickname,
                    clientName = client.ClientName,
                    isAdmin = client.IsAdmin,
                    hasAccount = client.HasAccount,
                    clientStatus = Mapper.MapStatus(client.ClientStatus),
                    answeredSurveys = client.SurveyResponses.Select(r => r.Survey.SurveyId).Distinct().ToList(),
                    assignments = client.ClientRelationXrefSenderClients.Count(),
                    senders = client.ClientRelationXrefRecipientClients.Count(),
                    notes = client.Notes.Count(),
                    tags = client.ClientTagXrefs.Select(tagXref => new Logic.Objects.Tag()
                    {
                        tagID = tagXref.TagId,
                        tagName = tagXref.Tag.TagName,
                    }).ToList(),

                }).AsNoTracking().ToListAsync();
            return clientList;
        }
        public async Task<HQClient> GetHeadquarterClientByID(Guid clientID)
        {
            HQClient logicHQClient = await santaContext.Clients
                .Select(client => new HQClient()
                {
                    clientID = client.ClientId,
                    email = client.Email,
                    nickname = client.Nickname,
                    clientName = client.ClientName,
                    isAdmin = client.IsAdmin,
                    hasAccount = client.HasAccount,
                    clientStatus = Mapper.MapStatus(client.ClientStatus),
                    answeredSurveys = client.SurveyResponses.Select(r => r.Survey.SurveyId).Distinct().ToList(),
                    assignments = client.ClientRelationXrefSenderClients.Count(),
                    senders = client.ClientRelationXrefRecipientClients.Count(),
                    notes = client.Notes.Count(),
                    tags = client.ClientTagXrefs.Select(tagXref => new Logic.Objects.Tag()
                    {
                        tagID = tagXref.TagId,
                        tagName = tagXref.Tag.TagName,
                    }).ToList(),
                }).AsNoTracking().FirstOrDefaultAsync(c => c.clientID == clientID);
            return logicHQClient;
        }
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
        public async Task UpdateClientByIDAsync(Logic.Objects.Client targetLogicClient)
        {
            Entities.Client contextOldClient = await santaContext.Clients.FirstOrDefaultAsync(c => c.ClientId == targetLogicClient.clientID);

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

            santaContext.Clients.Update(contextOldClient);
        }
        public async Task UpdateAssignmentProgressStatusByID(Guid assignmentID, Guid newAssignmentStatusID)
        {
            ClientRelationXref contextRelationship = await santaContext.ClientRelationXrefs.FirstOrDefaultAsync(crxf => crxf.ClientRelationXrefId == assignmentID);

            contextRelationship.AssignmentStatusId = newAssignmentStatusID;

            santaContext.ClientRelationXrefs.Update(contextRelationship);
        }
        public async Task DeleteClientByIDAsync(Guid clientID)
        {
            Data.Entities.Client contextClient = await santaContext.Clients.FirstOrDefaultAsync(c => c.ClientId == clientID);

            santaContext.Clients.Remove(contextClient);
        }
        public async Task DeleteRecieverXref(Guid clientID, Guid recipientID, Guid eventID)
        {
            Data.Entities.ClientRelationXref contextRelation = await santaContext.ClientRelationXrefs.FirstOrDefaultAsync(r => r.SenderClientId == clientID && r.RecipientClientId == recipientID && r.EventTypeId == eventID);
            List<ChatMessage> contextRelatedMessages = await santaContext.ChatMessages.Where(cm => cm.ClientRelationXrefId == contextRelation.ClientRelationXrefId).ToListAsync();
            santaContext.ChatMessages.RemoveRange(contextRelatedMessages);
            santaContext.ClientRelationXrefs.Remove(contextRelation);
        }
        #endregion

        #region Informational
        public async Task<InfoContainer> getClientInfoContainerByIDAsync(Guid clientID)
        {
            InfoContainer logicRelationshipContainer = await santaContext.Clients
                .Select(client => new InfoContainer()
                {
                    agentID = client.ClientId,
                    notes = client.Notes.Select(note => new Logic.Objects.Base_Objects.Note()
                    {
                        noteID = note.NoteId,
                        noteSubject = note.NoteSubject,
                        noteContents = note.NoteContents
                    }).ToList(),
                    assignments = client.ClientRelationXrefSenderClients.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = Mapper.MapClientMeta(xref.RecipientClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>(),
                        removable = xref.ChatMessages.Count > 0 ? false : true
                    }).ToList(),
                    senders = client.ClientRelationXrefRecipientClients.Select(xref => new RelationshipMeta()
                    {
                        relationshipClient = Mapper.MapClientMeta(xref.SenderClient),
                        eventType = Mapper.MapEvent(xref.EventType),
                        clientRelationXrefID = xref.ClientRelationXrefId,
                        assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                        tags = new List<Logic.Objects.Tag>(),
                        removable = xref.ChatMessages.Count > 0 ? false : true
                    }).ToList(),

                    responses = client.SurveyResponses.Select(surveyResponse => new Response()
                    {
                        surveyResponseID = surveyResponse.SurveyResponseId,
                        clientID = surveyResponse.ClientId,
                        surveyID = surveyResponse.SurveyId,
                        surveyOptionID = surveyResponse.SurveyOptionId,
                        responseText = surveyResponse.ResponseText,
                        responseEvent = Mapper.MapEvent(surveyResponse.Survey.EventType),
                        surveyQuestion = Mapper.MapQuestion(surveyResponse.SurveyQuestion)
                    }).ToList()
                }).AsNoTracking().FirstOrDefaultAsync(c => c.agentID == clientID);
            return logicRelationshipContainer;
        }
        public async Task<List<RelationshipMeta>> getClientAssignmentsInfoByIDAsync(Guid clientID)
        {
            List<RelationshipMeta> listLogicRelationshipMeta = await santaContext.ClientRelationXrefs.Where(crxr => crxr.SenderClientId == clientID)
                .Select(xref => new RelationshipMeta()
                {
                    clientRelationXrefID = xref.ClientRelationXrefId,
                    relationshipClient = Mapper.MapClientMeta(xref.RecipientClient),
                    eventType = Mapper.MapEvent(xref.EventType),
                    assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                    tags = new List<Logic.Objects.Tag>(),
                    removable = xref.ChatMessages.Count > 0
                }).ToListAsync();
            return listLogicRelationshipMeta;
        }
        public async Task<RelationshipMeta> getAssignmentRelationshipMetaByIDAsync(Guid xrefID)
        {
            RelationshipMeta logicRelationshipMeta = await santaContext.ClientRelationXrefs
                .Select(xref => new RelationshipMeta()
                {
                    clientRelationXrefID = xref.ClientRelationXrefId,
                    relationshipClient = Mapper.MapClientMeta(xref.RecipientClient),
                    eventType = Mapper.MapEvent(xref.EventType),
                    assignmentStatus = Mapper.MapAssignmentStatus(xref.AssignmentStatus),
                    tags = new List<Logic.Objects.Tag>(),
                    removable = xref.ChatMessages.Count > 0
                })
                .FirstOrDefaultAsync(crxr => crxr.clientRelationXrefID == xrefID);
            return logicRelationshipMeta;
        }
        public async Task<List<Guid>> getClientAssignmentXrefIDsByIDAsync(Guid clientID)
        {
            List<Guid> assignmentIDList = await santaContext.ClientRelationXrefs
                .Where(crxr => crxr.SenderClientId == clientID)
                .Select(crxr => crxr.ClientRelationXrefId)
                .Distinct()
                .ToListAsync();
            return assignmentIDList;
        }
        #endregion

        #region Status
        public async Task<List<Status>> GetAllClientStatus()
        {
            List<Logic.Objects.Status> logicStatusList = (await santaContext.ClientStatuses.ToListAsync()).Select(Mapper.MapStatus).ToList();
            return logicStatusList;
        }
        public async Task<Status> GetClientStatusByID(Guid clientStatusID)
        {
            Logic.Objects.Status logicStatus = Mapper.MapStatus(await santaContext.ClientStatuses
                .FirstOrDefaultAsync(s => s.ClientStatusId == clientStatusID));
            return logicStatus;
        }
        public async Task CreateStatusAsync(Status newStatus)
        {
            Data.Entities.ClientStatus contextStatus = Mapper.MapStatus(newStatus);
            await santaContext.ClientStatuses.AddAsync(contextStatus);
        }
        public async Task UpdateStatusByIDAsync(Status changedLogicStatus)
        {
            ClientStatus targetStatus = await santaContext.ClientStatuses.FirstOrDefaultAsync(s => s.ClientStatusId == changedLogicStatus.statusID);
            if (targetStatus == null)
            {
                throw new Exception("Client Status was not found. Update failed for status");
            }
            targetStatus.StatusDescription = changedLogicStatus.statusDescription;
            santaContext.ClientStatuses.Update(targetStatus);
        }
        public async Task DeleteStatusByIDAsync(Guid clientStatusID)
        {
            ClientStatus contextStatus = await santaContext.ClientStatuses.FirstOrDefaultAsync(s => s.ClientStatusId == clientStatusID);
            santaContext.ClientStatuses.Remove(contextStatus);
        }
        #endregion

        #region Utility
        public async Task SaveAsync()
        {
            await santaContext.SaveChangesAsync();
        }
        #endregion
    }
}
