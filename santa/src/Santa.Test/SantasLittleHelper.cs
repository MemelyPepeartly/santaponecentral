using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Santa.Data.Repository;
using Santa.Logic.Objects;
using Santa.Logic.Interfaces;
using Santa.Api.Controllers;

namespace Santa.Test
{
    public class SantasLittleHelper
    {
        public Mock<Santa.Logic.Interfaces.IRepository> Repository { get; private set; }

        //Controllers
        public ClientController ClientController { get; private set; }
        public EventController EventController { get; private set; }
        public SurveyController SurveyController { get; private set; }

        //Lists of Logic objects
        public List<Client> Clients { get; private set; }
        public List<Survey> Surveys { get; private set; }
        public List<Question> Questions { get; private set; }
        public List<Option> QuestionOptions { get; private set; }

        public SantasLittleHelper()
        {

        }
        private void SetUpClients()
        {

        }
        private void SetUpSurveys()
        {

        }
        private void SetUpQuestions()
        {

        }
        private void SetUpQuestionOptions()
        {

        }
    }
}
