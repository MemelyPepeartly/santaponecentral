﻿using System;
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
                    .Include(tx => tx.ClientTagXref)
                        .ThenInclude(t => t.Tag)
                    .Include(s => s.ClientStatus)
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
        public List<Logic.Objects.Tag> GetAllTags()
        {
            try
            {
                List<Logic.Objects.Tag> logicTags = new List<Logic.Objects.Tag>();
                logicTags = santaContext.Tag.Select(Mapper.MapTag).ToList();

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
                Data.Entities.Tag contextOldTag = await santaContext.Tag.FirstOrDefaultAsync(t => t.TagId == targetLogicTag.tagId);

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

        public List<Option> GetAllSurveyOption()
        {
            try
            {
                return santaContext.SurveyOption.Include(s=>s.SurveyQuestionOptionXref).Select(Mapper.MapSurveyOption).ToList();
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
        public List<Question> GetAllSurveyQuestions()
        {
            try
            {
                List<Question> listLogicQuestion = santaContext.SurveyQuestion
                    .Include(sq => sq.SurveyQuestionOptionXref)
                        .ThenInclude(so => so.SurveyOption)
                    .AsNoTracking()
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
                Logic.Objects.Response logicResponse = Mapper.MapResponse(await santaContext.SurveyResponse.FirstAsync(r => r.SurveyResponseId == surveyResponseID));
                return logicResponse;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public List<Logic.Objects.Response> GetAllSurveyResponsesByClientID(Guid clientID)
        {
            try
            {
                List<Response> listLogicResponse = santaContext.SurveyResponse.Select(Mapper.MapResponse).Where(r => r.clientID == clientID).ToList();
                return listLogicResponse;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
        public List<Logic.Objects.Response> GetAllSurveyResponses()
        {
            try
            {
                List<Response> listLogicResponse = santaContext.SurveyResponse.Select(Mapper.MapResponse).ToList();
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

        #endregion
    }
}
