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
        public Address TestAddress { get; private set; }

        public SantasLittleHelper()
        {
            SetUpClients();
            SetUpQuestions();
            SetUpQuestionOptions();
            SetUpSurveys();
            
            TestAddress = new Address
            {
                addressLineOne = "123 Test Street",
                addressLineTwo = "Apt 123",
                city = "TestCity",
                state = "TestState",
                postalCode = "12345",
                country = "USA"
            };
        }
        private void SetUpClients()
        {
            Clients = new List<Client>
            {
                new Client
                {
                    clientName = "Test Name 1",
                    email = "test1@test.com",
                    nickname = "Test Nickname 1",
                    address = TestAddress
                },
                new Client
                {
                    clientName = "Test Name 2",
                    email = "test2@test.com",
                    nickname = "Test Nickname 2",
                    address = TestAddress
                },
                new Client
                {
                    clientName = "Test Name 3",
                    email = "test3@test.com",
                    nickname = "Test Nickname 3",
                    address = TestAddress
                }
            };
        }
        private void SetUpQuestions()
        {
            Questions = new List<Question>
            {
                new Question
                {

                },
                new Question
                {

                }
            };
        }
        private void SetUpQuestionOptions()
        {

        }
        private void SetUpSurveys()
        {
            Surveys = new List<Survey>
            {
                new Survey
                {
                    surveyDescription = "Survey Description 1",
                    active = true,
                    surveyQuestions = Questions
                },
                new Survey
                {
                    surveyDescription = "Survey Description 2",
                    active = false,
                    surveyQuestions = Questions
                }
            };
        }
    }
}
