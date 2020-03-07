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
        private readonly SantaBaseContext santaContext;

        public Repository(SantaBaseContext _context)
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
        public Task<Logic.Objects.Client> DeleteClientByIDAsync()
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
                        .ThenInclude(crx => crx.RecipientClient)
                    .Include(sc => sc.ClientRelationXrefSenderClient)
                        .ThenInclude(crx => crx.SenderClient)
                    .Include(s => s.ClientStatus)
                    .Select(Mapper.MapClient).ToList();
                return clientList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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
                    .Include(s => s.ClientStatus)
                    .FirstOrDefaultAsync(c => c.ClientId == clientId));
                return logicClient;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void UpdateClientByIDAsync(Logic.Objects.Client targetLogicClient)
        {
            try
            {
                Data.Entities.Client mappedTargetContextClient = Mapper.MapClient(targetLogicClient);
                santaContext.Client.Update(mappedTargetContextClient);
            }
            catch(Exception e)
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
        public Task<Event> DeleteEventByIDAsync()
        {
            throw new NotImplementedException();
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
                throw new Exception(e.Message);
            }
        }
        public Task<Event> UpdateEventByIDAsync()
        {
            throw new NotImplementedException();
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
                        .ThenInclude(sqx => sqx.SurveyQuestion)
                            .ThenInclude(sq => sq.SurveyQuestionOptionXref)
                                .ThenInclude(so => so.SurveyOption)
                    .AsNoTracking()
                    .Select(Mapper.MapSurvey).ToList();

                return surveyList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region SurveyOption
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
                throw new Exception(e.Message);
            }
        }
        public Task<Question> DeleteSurveyOptionByIDAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Question> UpdateSurveyOptionByIDAsync()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region QuestionOption
        /// <summary>
        /// Creates the Xref between a Survey Question and a Survey Option
        /// </summary>
        /// <param name="newQuestionOption"></param>
        /// <returns></returns>
        public async Task CreateSurveyQuestionOptionXrefAsync(Option newQuestionOption)
        {
            try
            {
                Data.Entities.SurveyQuestionOptionXref contextQuestionOptionXref = Mapper.MapQuestionOptionXref(newQuestionOption);
                await santaContext.SurveyQuestionOptionXref.AddAsync(contextQuestionOptionXref);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        #endregion

        #region Question
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
        public async Task CreateSurveyQuestionXrefAsync(Question logicQuestion)
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
        public Task<Question> DeleteSurveyQuestionByIDAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Question> UpdateSurveyQuestionByIDAsync()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Response
        public Task<Logic.Objects.Response> CreateSurveyResponseAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Logic.Objects.Response> DeleteSurveyResponseByIDAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Logic.Objects.Response> GetSurveyResponseByIDAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Logic.Objects.Response> UpdateSurveyResponseByIDAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
        
        #region ClientStatus
        public List<Status> GetAllClientStatus()
        {
            try
            {
                List<Logic.Objects.Status> logicStatusList = santaContext.ClientStatus.Select(Mapper.MapStatus).ToList();
                return logicStatusList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
                throw new Exception(e.Message);
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
                throw new Exception(e.Message);
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
                throw new Exception(e.Message);
            }
        }

        public async Task DeleteStatusByIDAsync(Guid clientStatusID)
        {
            try
            {
                ClientStatus targetStatus = await santaContext.ClientStatus.FirstOrDefaultAsync(s => s.ClientStatusId == clientStatusID);
                santaContext.ClientStatus.Remove(targetStatus);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
        #endregion
    }
}
