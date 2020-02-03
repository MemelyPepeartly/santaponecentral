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
        Task<Logic.Objects.Event> CreateSurveyOptionAsync();
        Task<Logic.Objects.Event> GetSurveyOptionByIDAsync();
        Task<Logic.Objects.Event> UpdateSurveyOptionByIDAsync();
        Task<Logic.Objects.Event> DeleteSurveyOptionByIDAsync();
        #endregion

        #region SurveyQuestions
        Task<Logic.Objects.Event> CreateSurveyQuestionAsync();
        Task<Logic.Objects.Event> GetSurveyQuestionByIDAsync();
        Task<Logic.Objects.Event> UpdateSurveyQuestionByIDAsync();
        Task<Logic.Objects.Event> DeleteSurveyQuestionByIDAsync();
        #endregion

        #region SurveyResponses

        #endregion
    }
}