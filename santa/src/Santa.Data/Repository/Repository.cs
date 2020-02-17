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
        /// <summary>
        /// Creates a new client and adds it to the context
        /// </summary>
        /// <param name="newClient"></param>
        /// <returns></returns>
        public async Task CreateClient(Logic.Objects.Client newClient)
        {
            try
            {
                var contextClient = Mapper.MapClient(newClient);
                await santaContext.Client.AddAsync(contextClient);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<Event> CreateEventAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CreateSurvey(Logic.Objects.Survey newSurvey)
        {
            try
            {
                Data.Entities.Survey contextSurvey = Mapper.MapSurvey(newSurvey);
                await santaContext.Survey.AddAsync(contextSurvey);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<Question> CreateSurveyOptionAsync()
        {
            throw new NotImplementedException();
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
                throw new Exception(e.Message);
            }
            
        }
        /// <summary>
        /// Creates the cross reference between a survey and the questions it has and adds it to the context
        /// </summary>
        /// <param name="contextQuestion"></param>
        /// <returns></returns>
        public async Task CreateSurveyQuestionXref(Question logicQuestion)
        {
            try
            {
                Data.Entities.SurveyQuestionXref contextQuestionXref = Mapper.MapQuestionXref(logicQuestion);
                await santaContext.SurveyQuestionXref.AddAsync(contextQuestionXref);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        public Task<Logic.Objects.Response> CreateSurveyResponseAsync()
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

        public Task<Question> DeleteSurveyOptionByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Question> DeleteSurveyQuestionByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Logic.Objects.Response> DeleteSurveyResponseByIDAsync()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets a list of all clients
        /// </summary>
        /// <returns></returns>
        public List<Logic.Objects.Client> GetAllClients()
        {
            try
            {
                List<Logic.Objects.Client> clientList = new List<Logic.Objects.Client>();
                clientList = santaContext.Client
                    .Include(r => r.ClientRelationXrefRecipientClient)
                        .ThenInclude(u => u.RecipientClient)
                    .Include(s => s.ClientRelationXrefSenderClient)
                        .ThenInclude(u => u.SenderClient)
                    .Select(Mapper.MapClient).ToList();
                return clientList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Gets a list of all events
        /// </summary>
        /// <returns></returns>
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
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Gets all surveys
        /// </summary>
        /// <returns></returns>
        public List<Logic.Objects.Survey> GetAllSurveys()
        {
            try
            {
                List<Logic.Objects.Survey> surveyList = new List<Logic.Objects.Survey>();
                surveyList = santaContext.Survey
                    .Include(s => s.SurveyQuestionXref)
                        .ThenInclude(q => q.SurveyQuestion)
                    .Select(Mapper.MapSurvey).ToList();

                return surveyList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<Logic.Objects.Client> GetClientByEmailAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a client by their ID
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<Logic.Objects.Client> GetClientByID(Guid clientId)
        {
            try
            {
                Logic.Objects.Client logicClient = Mapper.MapClient(await santaContext.Client
                    .Include(s => s.ClientRelationXrefSenderClient)
                        .ThenInclude(u => u.SenderClient)
                    .Include(r => r.ClientRelationXrefRecipientClient)
                        .ThenInclude(u => u.RecipientClient)
                    .FirstOrDefaultAsync(c => c.ClientId == clientId));
                return logicClient;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<Event> GetEventByIDAsync()
        {
            throw new NotImplementedException();
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
                        .ThenInclude(q => q.SurveyQuestion)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.SurveyId == surveyId));
                return logicSurvey;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<Question> GetSurveyOptionByIDAsync()
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

        public Task<Logic.Objects.Response> GetSurveyResponseByIDAsync()
        {
            throw new NotImplementedException();
        }

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
                throw new Exception(e.Message);
            }
            
        }

        public Task<Logic.Objects.Client> UpdateClientByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> UpdateEventByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Question> UpdateSurveyOptionByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Question> UpdateSurveyQuestionByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Logic.Objects.Response> UpdateSurveyResponseByIDAsync()
        {
            throw new NotImplementedException();
        }
    }
}
