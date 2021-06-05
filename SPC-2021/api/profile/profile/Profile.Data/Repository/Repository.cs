using Microsoft.EntityFrameworkCore;
using Profile.Data.Entities;
using Profile.Logic.Interfaces;
using Profile.Logic.Objects;
using Profile.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Profile.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly SantaPoneCentralDatabaseContext santaContext;

        public Repository(SantaPoneCentralDatabaseContext _context)
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

        #region Profile
        public async Task<Logic.Objects.Profile> GetProfileByEmailAsync(string email)
        {
            Logic.Objects.Profile logicProfile = await santaContext.Clients
                .Select(client => new Logic.Objects.Profile()
                {
                    clientID = client.ClientId,
                    clientName = client.ClientName,
                    nickname = client.Nickname,
                    email = client.Email,
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
                    editable = client.ClientRelationXrefRecipientClients.Count > 0 ? false : true
                }).FirstOrDefaultAsync(c => c.email == email);

            return logicProfile;
        }
        public async Task<Logic.Objects.Profile> GetProfileByIDAsync(Guid clientID)
        {
            Logic.Objects.Profile logicProfile = await santaContext.Clients
                .Select(client => new Logic.Objects.Profile()
                {
                    clientID = client.ClientId,
                    clientName = client.ClientName,
                    nickname = client.Nickname,
                    email = client.Email,
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
                    editable = client.ClientRelationXrefRecipientClients.Count > 0 ? false : true
                }).FirstOrDefaultAsync(c => c.clientID == clientID);

            return logicProfile;
        }
        public async Task<List<ProfileAssignment>> GetProfileAssignments(Guid clientID)
        {
            List<ProfileAssignment> listLogicProfileAssignment = await santaContext.ClientRelationXrefs.Where(xr => xr.SenderClientId == clientID)
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
                    responses = xref.RecipientClient.SurveyResponses.Where(r => r.SurveyQuestion.SenderCanView == true).Select(surveyResponse => new Response()
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

        #region Utility
        public async Task SaveAsync()
        {
            await santaContext.SaveChangesAsync();
        }
        #endregion
    }
}
