﻿using System;
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
            SetUpSenders();
            SetUpClients();
            SetUpQuestionOptions();
            SetUpQuestions();
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

        private void SetUpClients()
        {
            Clients = new List<Client>
            {
                new Client
                {
                    clientName = "Test Name 1",
                    email = "test1@test.com",
                    nickname = "Test Nickname 1",
                    address = TestAddress,
                    senders = SenderGUIDList,
                    recipients = RecipientGUIDList
                },
                new Client
                {
                    clientName = "Test Name 2",
                    email = "test2@test.com",
                    nickname = "Test Nickname 2",
                    address = TestAddress,
                    senders = SenderGUIDList,
                    recipients = RecipientGUIDList
                },
                new Client
                {
                    clientName = "Test Name 3",
                    email = "test3@test.com",
                    nickname = "Test Nickname 3",
                    address = TestAddress,
                    senders = SenderGUIDList,
                    recipients = RecipientGUIDList
                }
            };
        }
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
