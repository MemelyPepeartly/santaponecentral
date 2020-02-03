using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Santa.Logic.Objects;

namespace Santa.Logic.Interfaces
{
    public interface IRepository
    {
        #region Client
        Task<Logic.Objects.Client> CreateClientAsync();
        Task<Logic.Objects.Client> GetClientByID();
        Task<Logic.Objects.Client> GetClientByEmailAsync();
        Task<Logic.Objects.Client> UpdateClientByIDAsync();
        Task<Logic.Objects.Client> DeleteClientByIDAsync();
        #endregion

        #region Event
        Task<Logic.Objects.Event> CreateEventAsync();
        Task<Logic.Objects.Event> GetEventByIDAsync();
        Task<Logic.Objects.Event> UpdateEventByIDAsync();
        Task<Logic.Objects.Event> DeleteEventByIDAsync();
        #endregion

        #region SurveyOptions


        #endregion

        #region SurveyQuestions

        #endregion

        #region SurveyResponses

        #endregion
    }
}