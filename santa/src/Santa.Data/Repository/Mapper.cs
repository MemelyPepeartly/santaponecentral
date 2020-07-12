﻿using System;
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
        /// <param name="contextClient"></param>
        /// <returns></returns>
        public static Logic.Objects.Client MapClient(Entities.Client contextClient)
        {
            Logic.Objects.Client logicClient = new Logic.Objects.Client()
            {
                clientID = contextClient.ClientId,
                email = contextClient.Email,
                nickname = contextClient.Nickname,
                clientName = contextClient.ClientName,
                address = new Address
                {
                    addressLineOne = contextClient.AddressLine1,
                    addressLineTwo = contextClient.AddressLine2,
                    city = contextClient.City,
                    country = contextClient.Country,
                    state = contextClient.State,
                    postalCode = contextClient.PostalCode
                },

                clientStatus = Mapper.MapStatus(contextClient.ClientStatus),

                responses = contextClient.SurveyResponse.Select(Mapper.MapResponse).ToList(),
                recipients = contextClient.ClientRelationXrefSenderClient.Select(Mapper.MapRelationSenderXref).ToList(),
                senders = contextClient.ClientRelationXrefRecipientClient.Select(Mapper.MapRelationRecipientXref).ToList(),
                tags = contextClient.ClientTagXref.Select(Mapper.MapTagRelationXref).ToList()
            };

            return logicClient;
        }
        public static Logic.Objects.Sender MapRelationRecipientXref(Data.Entities.ClientRelationXref contextRecipientXref)
        {
            Logic.Objects.Sender logicSender = new Sender()
            {
                senderClientID = contextRecipientXref.SenderClientId,
                senderEventTypeID = contextRecipientXref.EventTypeId,
                removable = contextRecipientXref.ChatMessage.Count > 0 ? false : true
            };
            return logicSender;
        }
        public static Logic.Objects.Recipient MapRelationSenderXref(Data.Entities.ClientRelationXref contextSenderXref)
        {
            Logic.Objects.Recipient logicRecipient = new Recipient()
            {
                recipientClientID = contextSenderXref.RecipientClientId,
                recipientEventTypeID = contextSenderXref.EventTypeId,
                removable = contextSenderXref.ChatMessage.Count > 0 ? false : true
            };
            return logicRecipient;
        }
        public static Logic.Objects.ProfileRecipient MapRelationProfileRecipientXref(Data.Entities.ClientRelationXref contextSenderXref, Data.Entities.Client contextRecipientClientData)
        {
            Logic.Objects.ProfileRecipient logicProfileRecipient = new ProfileRecipient()
            {
                recipientClientID = contextSenderXref.RecipientClientId,
                relationXrefID = contextSenderXref.ClientRelationXrefId,
                recipientEvent = Mapper.MapEvent(contextSenderXref.EventType),

                address = new Address
                {
                    addressLineOne = contextRecipientClientData.AddressLine1,
                    addressLineTwo = contextRecipientClientData.AddressLine2,
                    city = contextRecipientClientData.City,
                    country = contextRecipientClientData.Country,
                    state = contextRecipientClientData.State,
                    postalCode = contextRecipientClientData.PostalCode
                },
                name = contextRecipientClientData.ClientName,
                nickname = contextRecipientClientData.Nickname,
                responses = contextRecipientClientData.SurveyResponse.Select(Mapper.MapResponse).ToList()
            };
            return logicProfileRecipient;
        }

        public static ClientMeta MapClientMeta(Entities.Client contextClient)
        {
            Logic.Objects.ClientMeta logicMeta = new ClientMeta()
            {
                clientId = contextClient.ClientId,
                clientName = contextClient.ClientName,
                clientNickname = contextClient.Nickname
            };

            return logicMeta;
        }

        #endregion

        #region Profile
        public static Profile MapProfile(Entities.Client contextClient)
        {
            Logic.Objects.Profile logicProfile = new Logic.Objects.Profile()
            {
                clientID = contextClient.ClientId,
                clientName = contextClient.ClientName,
                nickname = contextClient.Nickname,
                email = contextClient.Email,
                address = new Address
                {
                    addressLineOne = contextClient.AddressLine1,
                    addressLineTwo = contextClient.AddressLine2,
                    city = contextClient.City,
                    country = contextClient.Country,
                    state = contextClient.State,
                    postalCode = contextClient.PostalCode
                },
                clientStatus = MapStatus(contextClient.ClientStatus),
                recipients = contextClient.ClientRelationXrefSenderClient.Select(s=> Mapper.MapRelationProfileRecipientXref(s ,s.RecipientClient)).ToList(),
                responses = contextClient.SurveyResponse.Select(Mapper.MapResponse).ToList()
            };

            return logicProfile;

        }
        #endregion

        #region Tag
        public static Logic.Objects.Tag MapTag(Data.Entities.Tag contextTag)
        {
            Logic.Objects.Tag logicTag = new Logic.Objects.Tag()
            {
                tagID = contextTag.TagId,
                tagName = contextTag.TagName
            };
            return logicTag;
        }
        public static Data.Entities.Tag MapTag(Logic.Objects.Tag logicTag)
        {
            Data.Entities.Tag contextTag = new Entities.Tag()
            {
                TagId = logicTag.tagID,
                TagName = logicTag.tagName
            };
            return contextTag;
        }
        public static Logic.Objects.Tag MapTagRelationXref(Data.Entities.ClientTagXref contextTagXref)
        {
            Logic.Objects.Tag logicTag = new Logic.Objects.Tag()
            {
                tagID = contextTagXref.TagId,
                tagName = contextTagXref.Tag.TagName
            };
            return logicTag;
        }
        #endregion

        #region Message

        public static Logic.Objects.Message MapMessage(ChatMessage contextMessage)
        {
            Logic.Objects.Message logicMessage = new Message()
            {
                chatMessageID = contextMessage.ChatMessageId,
                clientRelationXrefID = contextMessage.ClientRelationXrefId != null ? contextMessage.ClientRelationXrefId : null,
                recieverClient = new ClientMeta()
                {
                    clientId = contextMessage.MessageRecieverClientId != null ? contextMessage.MessageRecieverClientId : null,
                    clientName = contextMessage.MessageRecieverClientId != null ? contextMessage.MessageRecieverClient.ClientName : String.Empty,
                    clientNickname = contextMessage.MessageRecieverClientId != null ? contextMessage.MessageRecieverClient.Nickname : String.Empty
                },
                senderClient = new ClientMeta()
                {
                    clientId = contextMessage.MessageSenderClientId != null ? contextMessage.MessageSenderClientId : null,
                    clientName = contextMessage.MessageSenderClientId != null ? contextMessage.MessageSenderClient.ClientName : String.Empty,
                    clientNickname = contextMessage.MessageSenderClientId != null ? contextMessage.MessageSenderClient.Nickname : String.Empty
                },
                messageContent = contextMessage.MessageContent,
                dateTimeSent = contextMessage.DateTimeSent,
                isMessageRead = contextMessage.IsMessageRead
            };
            return logicMessage;
        }
        public static ChatMessage MapMessage(Message logicMessage)
        {
            Data.Entities.ChatMessage contextMessage = new ChatMessage()
            {
                ChatMessageId = logicMessage.chatMessageID,
                ClientRelationXrefId = logicMessage.clientRelationXrefID,
                MessageRecieverClientId = logicMessage.recieverClient.clientId,
                MessageSenderClientId = logicMessage.senderClient.clientId,
                MessageContent = logicMessage.messageContent,
                DateTimeSent = logicMessage.dateTimeSent,
                IsMessageRead = logicMessage.isMessageRead
            };
            return contextMessage;
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
        #region SurveyResponse
        public static Logic.Objects.Response MapResponse(Data.Entities.SurveyResponse contextResponse)
        {
            Logic.Objects.Response logicResponse = new Response()
            {
                surveyResponseID = contextResponse.SurveyResponseId,
                clientID = contextResponse.ClientId,
                surveyID = contextResponse.SurveyId,
                surveyQuestion = Mapper.MapQuestion(contextResponse.SurveyQuestion),
                surveyOptionID = contextResponse.SurveyOptionId,
                responseEvent = MapEvent(contextResponse.Survey.EventType),
                responseText = contextResponse.ResponseText
            };
            return logicResponse;
        }

        public static Data.Entities.SurveyResponse MapResponse(Logic.Objects.Response logicResponse)
        {
            Data.Entities.SurveyResponse contextResponse = new SurveyResponse()
            {
                SurveyResponseId = logicResponse.surveyResponseID,
                ClientId = logicResponse.clientID,
                SurveyId = logicResponse.surveyID,
                SurveyQuestionId = logicResponse.surveyQuestion.questionID,
                SurveyOptionId = logicResponse.surveyOptionID,
                ResponseText = logicResponse.responseText
            };
            return contextResponse;
        }
        #endregion
        #endregion
    }
}