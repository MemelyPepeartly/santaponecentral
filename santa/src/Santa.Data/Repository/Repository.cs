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

        public Task<Logic.Objects.Client> GetClientByEmailAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Logic.Objects.Client> GetClientByID()
        {
            throw new NotImplementedException();
        }

        public Task<Event> GetEventByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> GetSurveyOptionByIDAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event> GetSurveyQuestionByIDAsync()
        {
            throw new NotImplementedException();
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
