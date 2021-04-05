using Microsoft.EntityFrameworkCore;
using Search.Data.Entities;
using Search.Logic.Constants;
using Search.Logic.Interfaces;
using Search.Logic.Objects;
using Search.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly SantaPoneCentralDatabaseContext santaContext;

        public Repository(SantaPoneCentralDatabaseContext _context)
        {
            santaContext = _context ?? throw new ArgumentNullException(nameof(_context));
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
        #endregion

        #region Utility
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
                    .Where(c => !searchQuery.nicknames.Any() || searchQuery.nicknames.All(queryNickname => c.nickname == queryNickname))
                    .Where(c => !searchQuery.emails.Any() || searchQuery.emails.All(queryEmail => c.email == queryEmail))
                    .Where(c => !searchQuery.responses.Any() || searchQuery.responses.All(queryResponse => c.responses.Any(r => r.responseText.Contains(queryResponse))))
                    .Where(c => !searchQuery.cardAssignments.Any() || searchQuery.cardAssignments.All(queryAssignmentAmount => c.cardAssignments == queryAssignmentAmount))
                    .Where(c => !searchQuery.giftAssignments.Any() || searchQuery.giftAssignments.All(queryAssignmentAmount => c.giftAssignments == queryAssignmentAmount)).ToList();

            }
            else
            {
                matchingClients = allClientList
                    .Where(c => !searchQuery.tags.Any() || searchQuery.tags.Any(queryTagID => c.tags.Any(t => t.tagID == queryTagID)))
                    .Where(c => !searchQuery.statuses.Any() || searchQuery.statuses.Any(queryStatusID => c.clientStatus.statusID == queryStatusID))
                    .Where(c => !searchQuery.events.Any() || searchQuery.events.Any(queryEventID => c.responses.Any(r => r.responseEvent.eventTypeID == queryEventID)))
                    .Where(c => !searchQuery.names.Any() || searchQuery.names.Any(queryName => c.clientName == queryName))
                    .Where(c => !searchQuery.nicknames.Any() || searchQuery.nicknames.Any(queryNickname => c.nickname == queryNickname))
                    .Where(c => !searchQuery.emails.Any() || searchQuery.emails.Any(queryEmail => c.email == queryEmail))
                    .Where(c => !searchQuery.responses.Any() || searchQuery.responses.Any(queryResponse => c.responses.Any(r => r.responseText.Contains(queryResponse))))
                    .Where(c => !searchQuery.cardAssignments.Any() || searchQuery.cardAssignments.Any(queryAssignmentAmount => c.cardAssignments == queryAssignmentAmount))
                    .Where(c => !searchQuery.giftAssignments.Any() || searchQuery.giftAssignments.Any(queryAssignmentAmount => c.giftAssignments == queryAssignmentAmount)).ToList();
            }

            return matchingClients;
        }
#endregion
    }
}
