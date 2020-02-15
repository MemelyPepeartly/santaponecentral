using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly SantaBaseContext santaContext;

        public Repository(SantaBaseContext _context)
        {
            santaContext = _context ?? throw new ArgumentNullException(nameof(_context));
        }
        public Task<Logic.Objects.Client> CreateClientAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> CreateEventAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> CreateSurveyOptionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> CreateSurveyQuestionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> CreateSurveyResponseAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Logic.Objects.Client> DeleteClientByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> DeleteEventByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> DeleteSurveyOptionByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> DeleteSurveyQuestionByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> DeleteSurveyResponseByIDAsync()
        {
            throw new NotImplementedException();
        }

        public List<Logic.Objects.Client> GetAllClients()
        {
            try
            {
                List<Logic.Objects.Client> clientList = new List<Logic.Objects.Client>();
                clientList = santaContext.Client.Select(Mapper.MapClient).ToList();
                return clientList;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Event> GetAllEvents()
        {
            try
            {
                List<Logic.Objects.Event> eventList = new List<Logic.Objects.Event>();
                eventList = santaContext.EventType.Select(Mapper.MapEvent).ToList();
                return eventList;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Logic.Objects.Survey> GetAllSurveys()
        {
            try
            {
                List<Logic.Objects.Survey> surveyList = new List<Logic.Objects.Survey>();
                surveyList = santaContext.Survey.Select(Mapper.MapSurvey).ToList();
                return surveyList;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Task<Logic.Objects.Client> GetClientByEmailAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Logic.Objects.Client> GetClientByID(Guid clientId)
        {
            try
            {
                Logic.Objects.Client logicClient = Mapper.MapClient(await santaContext.Client
                    .AsNoTracking()
                    .Include(s => s.ClientRelationXrefSenderClient)
                    .Include(r => r.ClientRelationXrefRecipientClient)
                    .FirstOrDefaultAsync(c => c.ClientId == clientId));
                return logicClient;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Task<Event> GetEventByIDAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Logic.Objects.Survey> GetSurveyByID(Guid surveyId)
        {
            try
            {
                Logic.Objects.Survey logicSurvey = Mapper.MapSurvey(await santaContext.Survey
                    .Include(s => s.SurveyQuestionXref)
                        .ThenInclude(q => q.SurveyQuestion)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.SurveyId == surveyId));
                return logicSurvey;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Task<Event> GetSurveyOptionByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Question>> GetSurveyQuestionsBySurveyIDAsync(Guid surveyId)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
                return null;
            }
        }

        public Task<Event> GetSurveyResponseByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Logic.Objects.Client> UpdateClientByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> UpdateEventByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> UpdateSurveyOptionByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> UpdateSurveyQuestionByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> UpdateSurveyResponseByIDAsync()
        {
            throw new NotImplementedException();
        }
    }
}
