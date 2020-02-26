using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Santa.Data.Repository;
using Santa.Logic.Objects;
using Santa.Logic.Interfaces;
using Santa.Api.Controllers;
using Microsoft.AspNetCore.Http;

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
        public List<Recipient> Recipients { get; private set; }
        public List<Sender> Senders { get; private set; }
        public List<Guid> RecipientGUIDList { get; private set; }
        public List<Guid> SenderGUIDList { get; private set; }
        public List<Survey> Surveys { get; private set; }
        public List<Question> Questions { get; private set; }
        public List<Option> QuestionOptions { get; private set; }
        public Address TestAddress { get; private set; }

        public SantasLittleHelper()
        {
            SetUpRecipients();
            SetUpAddress();
            SetUpSenders();
            SetUpClients();
            SetUpQuestionOptions();
            SetUpQuestions();
            SetUpSurveys();
            SetUpMocks();
        }
        /// <summary>
        /// Sets up a test address
        /// </summary>
        private void SetUpAddress()
        {
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
        /// <summary>
        /// Sets up recipients
        /// </summary>
        private void SetUpRecipients()
        {
            Recipients = new List<Recipient>
            {
                new Recipient
                {
                    recipientClientID = Guid.NewGuid(),
                    recipientName = "Test Recipient 1"
                },
                new Recipient
                {
                    recipientClientID = Guid.NewGuid(),
                    recipientName = "Test Recipient 2"
                }
            };
            RecipientGUIDList = new List<Guid>();
            RecipientGUIDList.Add(Recipients[0].recipientClientID);
            RecipientGUIDList.Add(Recipients[1].recipientClientID);
        }
        /// <summary>
        /// Sets up senders
        /// </summary>
        private void SetUpSenders()
        {
            Senders = new List<Sender>
            {
                new Sender
                {
                    senderClientID = Guid.NewGuid(),
                    senderName = "Test Sender 1"
                },
                new Sender
                {
                    senderClientID = Guid.NewGuid(),
                    senderName = "Test Sender 2"
                }
            };
            SenderGUIDList = new List<Guid>();
            SenderGUIDList.Add(Senders[0].senderClientID);
            SenderGUIDList.Add(Senders[1].senderClientID);
        }
        /// <summary>
        /// Sets up clients
        /// </summary>
        private void SetUpClients()
        {
            Clients = new List<Client>
            {
                new Client
                {
                    clientID = Guid.NewGuid(),
                    clientName = "Test Name 1",
                    email = "test1@test.com",
                    nickname = "Test Nickname 1",
                    address = TestAddress,
                    senders = SenderGUIDList,
                    recipients = RecipientGUIDList,
                    clientStatusID = Guid.NewGuid(),
                    clientStatusDescription = "Test Status Description 1"
                },
                new Client
                {
                    clientID = Guid.NewGuid(),
                    clientName = "Test Name 2",
                    email = "test2@test.com",
                    nickname = "Test Nickname 2",
                    address = TestAddress,
                    senders = SenderGUIDList,
                    recipients = RecipientGUIDList,
                    clientStatusID = Guid.NewGuid(),
                    clientStatusDescription = "Test Status Description 2"
                },
                new Client
                {
                    clientID = Guid.NewGuid(),
                    clientName = "Test Name 3",
                    email = "test3@test.com",
                    nickname = "Test Nickname 3",
                    address = TestAddress,
                    senders = SenderGUIDList,
                    recipients = RecipientGUIDList,
                    clientStatusID = Guid.NewGuid(),
                    clientStatusDescription = "Test Status Description 3"
                }
            };
        }
        /// <summary>
        /// Sets up question options
        /// </summary>
        private void SetUpQuestionOptions()
        {
            QuestionOptions = new List<Option>
            {
                new Option(Guid.NewGuid())
                {
                    displayText = "Option 1 Text",
                    surveyOptionValue = "Option 1 Value",
                    sortOrder = "asc",
                    isActive = true
                },
                new Option(Guid.NewGuid())
                {
                    displayText = "Option 2 Text",
                    surveyOptionValue = "Option 2 Value",
                    sortOrder = "asc",
                    isActive = false
                }
            };
        }
        /// <summary>
        /// Sets up questions
        /// </summary>
        private void SetUpQuestions()
        {
            Questions = new List<Question>
            {
                new Question(Guid.NewGuid())
                {
                    questionText = "Question 1 Text",
                    isActive = true,
                    isSurveyOptionList = true,
                    surveyOptionList = QuestionOptions,
                    sortOrder = "asc"
                },
                new Question(Guid.NewGuid())
                {
                    questionText = "Question 2 Text",
                    isActive = false,
                    isSurveyOptionList = false,
                    sortOrder = "asc"
                }
            };
        }
        /// <summary>
        /// Sets up surveys
        /// </summary>
        private void SetUpSurveys()
        {
            Surveys = new List<Survey>
            {
                new Survey
                {
                    surveyID = Guid.NewGuid(),
                    surveyDescription = "Survey Description 1",
                    active = true,
                    surveyQuestions = Questions,
                    eventTypeID = Guid.NewGuid()
                },
                new Survey
                {
                    surveyID = Guid.NewGuid(),
                    surveyDescription = "Survey Description 2",
                    active = false,
                    surveyQuestions = Questions,
                    eventTypeID = Guid.NewGuid()
                }
            };
        }
        /// <summary>
        /// Sets up Mocks
        /// </summary>
        private void SetUpMocks()
        {
            Repository = new Mock<IRepository>();

            ClientController = new ClientController(Repository.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            EventController = new EventController(Repository.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            SurveyController = new SurveyController(Repository.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }
    }
}
