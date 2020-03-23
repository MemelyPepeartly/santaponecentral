using System;
using System.Linq;
using System.Linq.Expressions;
using Santa.Data.Entities;
using Santa.Logic.Objects;

namespace Santa.Data.Repository
{
    public static class Mapper
    {

        #region Client
        /// <summary>
        /// maps a logic client to a context client
        /// </summary>
        /// <param name="logicClient"></param>
        /// <returns></returns>
        public static Data.Entities.Client MapClient(Logic.Objects.Client logicClient)
        {
            Entities.Client contextClient = new Entities.Client()
            {
                ClientId = logicClient.clientID,
                ClientName = logicClient.clientName,
                Email = logicClient.email,
                Nickname = logicClient.nickname,
                ClientStatusId = logicClient.clientStatus.statusID,
                AddressLine1 = logicClient.address.addressLineOne,
                AddressLine2 = logicClient.address.addressLineTwo,
                City = logicClient.address.city,
                State = logicClient.address.state,
                PostalCode = logicClient.address.postalCode,
                Country = logicClient.address.country,

            };
            return contextClient;
        }

        /// <summary>
        /// Maps a context client to a logic client
        /// </summary>
        /// <param name="contextCharacter"></param>
        /// <returns></returns>
        public static Logic.Objects.Client MapClient(Entities.Client contextCharacter)
        {
            Logic.Objects.Client logicClient = new Logic.Objects.Client()
            {
                clientID = contextCharacter.ClientId,      
                email = contextCharacter.Email,
                nickname = contextCharacter.Nickname,
                clientName = contextCharacter.ClientName,
                address = new Address
                {
                    addressLineOne = contextCharacter.AddressLine1,
                    addressLineTwo = contextCharacter.AddressLine2,
                    city = contextCharacter.City,
                    country = contextCharacter.Country,
                    state = contextCharacter.State,
                    postalCode = contextCharacter.PostalCode
                },
                
                clientStatus = Mapper.MapStatus(contextCharacter.ClientStatus),
                
                senders = contextCharacter.ClientRelationXrefSenderClient.Select(Mapper.MapRelationSenderXref).ToList(),
                recipients = contextCharacter.ClientRelationXrefRecipientClient.Select(Mapper.MapRelationRecipientXref).ToList()
            };

            return logicClient;
        }
        public static Logic.Objects.Recipient MapRelationRecipientXref(Data.Entities.ClientRelationXref contextRecipientXref)
        {
            Logic.Objects.Recipient logicRecipient = new Recipient()
            {
                recipientClientID = contextRecipientXref.RecipientClientId,
                recipientNickname = contextRecipientXref.RecipientClient.Nickname,
                recipientEventTypeID = contextRecipientXref.EventTypeId
            };
            return logicRecipient;
        }
        public static Logic.Objects.Sender MapRelationSenderXref(Data.Entities.ClientRelationXref contextSenderXref)
        {
            Logic.Objects.Sender logicSender = new Sender()
            {
                senderClientID = contextSenderXref.SenderClientId,
                senderNickname = contextSenderXref.SenderClient.Nickname,
                senderEventTypeID = contextSenderXref.EventTypeId
            };
            return logicSender;
        }

        #endregion
        #region Status
        public static Status MapStatus(ClientStatus contextStatus)
        {
            Logic.Objects.Status logicStatus = new Status()
            {
                statusID = contextStatus.ClientStatusId,
                statusDescription = contextStatus.StatusDescription
            };
            return logicStatus;
        }
        public static Data.Entities.ClientStatus MapStatus(Logic.Objects.Status logicStatus)
        {
            Entities.ClientStatus contextStatus = new ClientStatus()
            {
                ClientStatusId = logicStatus.statusID,
                StatusDescription = logicStatus.statusDescription
            };
            return contextStatus;
        }
        #endregion
        #region Event

        public static Logic.Objects.Event MapEvent(Entities.EventType contextEventType)
        {
            Logic.Objects.Event logicEvent = new Logic.Objects.Event()
            {
                eventTypeID = contextEventType.EventTypeId,
                eventDescription = contextEventType.EventDescription,
                active = contextEventType.IsActive
            };
            return logicEvent;
        }
        public static Entities.EventType MapEvent(Logic.Objects.Event logicEvent)
        {
            Entities.EventType contextEvent = new EventType()
            {
                EventTypeId = logicEvent.eventTypeID,
                EventDescription = logicEvent.eventDescription,
                IsActive = logicEvent.active
            };
            return contextEvent;
        }
        #endregion
        #region Survey
        /// <summary>
        /// Maps context survey to a logic survey
        /// </summary>
        /// <param name="contextSurvey"></param>
        /// <returns></returns>
        public static Logic.Objects.Survey MapSurvey(Entities.Survey contextSurvey)
      {
            Logic.Objects.Survey logicSurvey = new Logic.Objects.Survey()
            {
                surveyID = contextSurvey.SurveyId,
                eventTypeID = contextSurvey.EventTypeId,
                surveyDescription = contextSurvey.SurveyDescription,
                active = contextSurvey.IsActive,
                surveyQuestions = contextSurvey.SurveyQuestionXref.Select(q => Mapper.MapQuestion(q.SurveyQuestion)).ToList(),
            };
            return logicSurvey;
        }

        public static Entities.Survey MapSurvey(Logic.Objects.Survey logicSurvey)
        {
            Data.Entities.Survey contextSurvey = new Entities.Survey()
            {
                SurveyId = logicSurvey.surveyID,
                EventTypeId = logicSurvey.eventTypeID,
                SurveyDescription = logicSurvey.surveyDescription,
                IsActive = logicSurvey.active
            };
            return contextSurvey;
        }
        #endregion
        #region Question
        /// <summary>
        /// Maps a context question to a logic question
        /// </summary>
        /// <param name="contextSurveyQuestion"></param>
        /// <returns></returns>
        public static Logic.Objects.Question MapQuestion(Entities.SurveyQuestion contextSurveyQuestion)
        {

            Logic.Objects.Question logicQuestion = new Question()
            {
                questionID = contextSurveyQuestion.SurveyQuestionId,
                questionText = contextSurveyQuestion.QuestionText,
                isSurveyOptionList = contextSurveyQuestion.IsSurveyOptionList,
                surveyOptionList = contextSurveyQuestion.SurveyQuestionOptionXref.Select(Mapper.MapSurveyQuestionOption).ToList()
            };
            return logicQuestion;
        }

        /// <summary>
        /// maps a logic question to a context question
        /// </summary>
        /// <param name="newQuestion"></param>
        /// <returns></returns>
        public static Data.Entities.SurveyQuestion MapQuestion(Logic.Objects.Question newQuestion)
        {
            
            Entities.SurveyQuestion contextQuestion = new SurveyQuestion()
            {
                SurveyQuestionId = newQuestion.questionID,
                QuestionText = newQuestion.questionText,
                IsSurveyOptionList = newQuestion.isSurveyOptionList
            };
            return contextQuestion;
        }
        public static SurveyQuestionXref MapQuestionXref(Logic.Objects.Question logicQuestion)
        {
            Data.Entities.SurveyQuestionXref contextQuestionXref = new SurveyQuestionXref()
            {
                SurveyQuestionId = logicQuestion.questionID,
#warning might be a cause of problems here
            };
            return contextQuestionXref;
        }
        #region SurveyOption
        /// <summary>
        /// Takes a logic survey option and returns a context survey option
        /// </summary>
        /// <param name="newSurveyOption"></param>
        /// <returns></returns>
        public static SurveyOption MapSurveyOption(Option newSurveyOption)
        {
            Entities.SurveyOption contextSurveyOption = new SurveyOption()
            {
                SurveyOptionId = newSurveyOption.surveyOptionID,
                DisplayText = newSurveyOption.displayText,
                SurveyOptionValue = newSurveyOption.surveyOptionValue
            };
            return contextSurveyOption;
        }
        public static Option MapSurveyOption(SurveyOption contextSurveyOption)
        {
            Logic.Objects.Option logicSurveyOption = new Option()
            {
                surveyOptionID = contextSurveyOption.SurveyOptionId,
                displayText = contextSurveyOption.DisplayText,
                surveyOptionValue = contextSurveyOption.SurveyOptionValue
            };
            return logicSurveyOption;
        }
        #endregion
        #region QuestionOptionXref

        public static SurveyQuestionOptionXref MapQuestionOptionXref(Option newQuestionOption)
        {
            Data.Entities.SurveyQuestionOptionXref contextQuestionOptionXref = new SurveyQuestionOptionXref()
            {
                SurveyOptionId = newQuestionOption.surveyOptionID
#warning Gonna cause problems here probably too
            };
            return contextQuestionOptionXref;
        }
        /// <summary>
        /// Takes a context question option Xref and returns a logic option 
        /// </summary>
        /// <param name="contextQuestionOption"></param>
        /// <returns></returns>
        public static Logic.Objects.Option MapSurveyQuestionOption(SurveyQuestionOptionXref contextQuestionOption)
        {
            Logic.Objects.Option logicOption = new Option()
            {
                surveyOptionID = contextQuestionOption.SurveyOption.SurveyOptionId,
                displayText = contextQuestionOption.SurveyOption.DisplayText,
                surveyOptionValue = contextQuestionOption.SurveyOption.SurveyOptionValue
            };
            return logicOption;
        }
        #endregion
        #endregion
    }
}