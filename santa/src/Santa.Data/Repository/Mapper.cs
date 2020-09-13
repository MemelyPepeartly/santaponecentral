using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using Santa.Data.Entities;
using Santa.Logic.Constants;
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
                IsAdmin = logicClient.isAdmin,
                HasAccount = logicClient.hasAccount
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
                isAdmin = contextClient.IsAdmin,
                hasAccount = contextClient.HasAccount,
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
        /// <summary>
        /// Maps a context recipient relationship xref to a sender logic object
        /// </summary>
        /// <param name="contextRecipientXref"></param>
        /// <returns></returns>
        public static Logic.Objects.Sender MapRelationRecipientXref(Data.Entities.ClientRelationXref contextRecipientXref)
        {
            Logic.Objects.Sender logicSender = new Sender()
            {
                senderClientID = contextRecipientXref.SenderClientId,
                senderEventTypeID = contextRecipientXref.EventTypeId,
                assignmentStatus = MapAssignmentStatus(contextRecipientXref.AssignmentStatus),
                completed = contextRecipientXref.Completed,
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
                assignmentStatus = MapAssignmentStatus(contextSenderXref.AssignmentStatus),
                completed = contextSenderXref.Completed,
                removable = contextSenderXref.ChatMessage.Count > 0 ? false : true
            };
            return logicRecipient;
        }
        public static Logic.Objects.ProfileRecipient MapRelationProfileRecipientXref(Data.Entities.ClientRelationXref contextSenderXref, Data.Entities.Client contextRecipientClientData)
        {
            Logic.Objects.ProfileRecipient logicProfileRecipient = new ProfileRecipient()
            {
                recipientClientID = contextSenderXref.RecipientClientId,
                name = contextRecipientClientData.ClientName,
                nickname = contextRecipientClientData.Nickname,
                relationXrefID = contextSenderXref.ClientRelationXrefId,
                recipientEvent = Mapper.MapEvent(contextSenderXref.EventType),
                assignmentStatus = MapAssignmentStatus(contextSenderXref.AssignmentStatus),

                address = new Address
                {
                    addressLineOne = contextRecipientClientData.AddressLine1,
                    addressLineTwo = contextRecipientClientData.AddressLine2,
                    city = contextRecipientClientData.City,
                    country = contextRecipientClientData.Country,
                    state = contextRecipientClientData.State,
                    postalCode = contextRecipientClientData.PostalCode
                },
                completed = contextSenderXref.Completed,
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
                clientNickname = contextClient.Nickname,
                hasAccount = contextClient.HasAccount
            };

            return logicMeta;
        }
        public static ClientMeta MapClientMeta(Logic.Objects.Client logicClient)
        {
            Logic.Objects.ClientMeta logicMeta = new ClientMeta()
            {
                clientId = logicClient.clientID,
                clientName = logicClient.clientName,
                clientNickname = logicClient.nickname,
                hasAccount = logicClient.hasAccount
            };

            return logicMeta;
        }

        #endregion

        #region Assignment Status
        public static Entities.AssignmentStatus MapAssignmentStatus(Logic.Objects.AssignmentStatus logicAssignmentStatus)
        {
            Entities.AssignmentStatus contextAssignmentStatus = new Entities.AssignmentStatus()
            {
                AssignmentStatusId = logicAssignmentStatus.assignmentStatusID,
                AssignmentStatusName = logicAssignmentStatus.assignmentStatusName,
                AssignmentStatusDescription = logicAssignmentStatus.assignmentStatusDescription
            };

            return contextAssignmentStatus;
        }

        public static Logic.Objects.AssignmentStatus MapAssignmentStatus(Entities.AssignmentStatus contextAssignmentStatus)
        {
            Logic.Objects.AssignmentStatus logicAssignmentStatus = new Logic.Objects.AssignmentStatus()
            {
                assignmentStatusID = contextAssignmentStatus.AssignmentStatusId,
                assignmentStatusName = contextAssignmentStatus.AssignmentStatusName,
                assignmentStatusDescription = contextAssignmentStatus.AssignmentStatusDescription
            };

            return logicAssignmentStatus;
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
                recipients = contextClient.ClientRelationXrefSenderClient.Select(s => Mapper.MapRelationProfileRecipientXref(s, s.RecipientClient)).ToList(),
                responses = contextClient.SurveyResponse.Select(Mapper.MapResponse).ToList(),
                editable = contextClient.ClientRelationXrefRecipientClient.Count > 0 ? false : true
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
                tagName = contextTag.TagName,
                deletable = contextTag.ClientTagXref.Count > 0 ? false : true,
                tagImmutable = contextTag.TagName == Constants.MASS_MAILER_TAG || contextTag.TagName == Constants.MASS_MAIL_RECIPIENT_TAG || contextTag.TagName == Constants.GRINCH_TAG ? true : false
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
                    clientId = contextMessage.MessageReceiverClientId != null ? contextMessage.MessageReceiverClientId : null,
                    clientName = contextMessage.MessageReceiverClientId != null ? contextMessage.MessageReceiverClient.ClientName : String.Empty,
                    clientNickname = contextMessage.MessageReceiverClientId != null ? contextMessage.MessageReceiverClient.Nickname : String.Empty
                },
                senderClient = new ClientMeta()
                {
                    clientId = contextMessage.MessageSenderClientId != null ? contextMessage.MessageSenderClientId : null,
                    clientName = contextMessage.MessageSenderClientId != null ? contextMessage.MessageSenderClient.ClientName : String.Empty,
                    clientNickname = contextMessage.MessageSenderClientId != null ? contextMessage.MessageSenderClient.Nickname : String.Empty
                },
                messageContent = contextMessage.MessageContent,
                dateTimeSent = contextMessage.DateTimeSent,
                isMessageRead = contextMessage.IsMessageRead,
                fromAdmin = contextMessage.FromAdmin
            };
            return logicMessage;
        }

        public static ChatMessage MapMessage(Message logicMessage)
        {
            Data.Entities.ChatMessage contextMessage = new ChatMessage()
            {
                ChatMessageId = logicMessage.chatMessageID,
                ClientRelationXrefId = logicMessage.clientRelationXrefID,
                MessageReceiverClientId = logicMessage.recieverClient.clientId,
                MessageSenderClientId = logicMessage.senderClient.clientId,
                MessageContent = logicMessage.messageContent,
                DateTimeSent = logicMessage.dateTimeSent,
                IsMessageRead = logicMessage.isMessageRead,
                FromAdmin = logicMessage.fromAdmin
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
                active = contextEventType.IsActive,
                removable = contextEventType.ClientRelationXref.Count == 0 && contextEventType.Survey.Count == 0,
                immutable = contextEventType.EventDescription == Constants.CARD_EXCHANGE_EVENT || contextEventType.EventDescription == Constants.GIFT_EXCHANGE_EVENT
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
                removable = contextSurvey.SurveyQuestionXref.Count == 0
            };
            return logicSurvey;
        }
        /// <summary>
        /// Maps a logic survey to an entity survey type
        /// </summary>
        /// <param name="logicSurvey"></param>
        /// <returns></returns>
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
                senderCanView = contextSurveyQuestion.SenderCanView,
                surveyOptionList = contextSurveyQuestion.SurveyQuestionOptionXref.Select(Mapper.MapSurveyQuestionOption).ToList(),
                removable = contextSurveyQuestion.SurveyResponse.Count == 0 && contextSurveyQuestion.SurveyQuestionOptionXref.Count == 0
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
        /// <param name="logicSurveyOption"></param>
        /// <returns></returns>
        public static SurveyOption MapSurveyOption(Option logicSurveyOption)
        {
            Entities.SurveyOption contextSurveyOption = new SurveyOption()
            {
                SurveyOptionId = logicSurveyOption.surveyOptionID,
                DisplayText = logicSurveyOption.displayText,
                SurveyOptionValue = logicSurveyOption.surveyOptionValue
            };
            return contextSurveyOption;
        }
        public static Option MapSurveyOption(SurveyOption contextSurveyOption)
        {
            Logic.Objects.Option logicSurveyOption = new Option()
            {
                surveyOptionID = contextSurveyOption.SurveyOptionId,
                displayText = contextSurveyOption.DisplayText,
                surveyOptionValue = contextSurveyOption.SurveyOptionValue,
                removable = contextSurveyOption.SurveyResponse.Count == 0
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

        #region Board Entry
        /// <summary>
        /// Maps a context board entry to a logic board entry
        /// </summary>
        /// <param name="contextBoardEntry"></param>
        /// <returns></returns>
        public static Logic.Objects.BoardEntry MapBoardEntry(Data.Entities.BoardEntry contextBoardEntry)
        {
            Logic.Objects.BoardEntry logicBoardEntry = new Logic.Objects.BoardEntry()
            {
                boardEntryID = contextBoardEntry.BoardEntryId,
                entryType = Mapper.MapEntryType(contextBoardEntry.EntryType),
                threadNumber = contextBoardEntry.ThreadNumber,
                postNumber = contextBoardEntry.PostNumber,
                postDescription = contextBoardEntry.PostDescription,
                dateTimeEntered = contextBoardEntry.DateTimeEntered
            };
            return logicBoardEntry;
        }
        /// <summary>
        /// Maps a logic board entry to a context board entry
        /// </summary>
        /// <param name="logicBoardEntry"></param>
        /// <returns></returns>
        public static Data.Entities.BoardEntry MapBoardEntry(Logic.Objects.BoardEntry logicBoardEntry)
        {
            Data.Entities.BoardEntry contextBoardEntry = new Entities.BoardEntry()
            {
                BoardEntryId = logicBoardEntry.boardEntryID,
                EntryTypeId = logicBoardEntry.entryType.entryTypeID,
                ThreadNumber = logicBoardEntry.threadNumber,
                PostNumber = logicBoardEntry.postNumber,
                PostDescription = logicBoardEntry.postDescription,
                DateTimeEntered = logicBoardEntry.dateTimeEntered
            };
            return contextBoardEntry;
        }
        #endregion

        #region Entry Type
        /// <summary>
        /// Maps a context entry type into a logic entry type
        /// </summary>
        /// <param name="contextEntryType"></param>
        /// <returns></returns>
        public static Logic.Objects.EntryType MapEntryType(Data.Entities.EntryType contextEntryType)
        {
            Logic.Objects.EntryType logicEntryType = new Logic.Objects.EntryType()
            {
                entryTypeID = contextEntryType.EntryTypeId,
                entryTypeName = contextEntryType.EntryTypeName,
                entryTypeDescription = contextEntryType.EntryTypeDescription,
                adminOnly = contextEntryType.AdminOnly
            };
            return logicEntryType;
        }
        /// <summary>
        /// Maps a logic entry type into a context entry type
        /// </summary>
        /// <param name="logicEntryType"></param>
        /// <returns></returns>
        public static Data.Entities.EntryType MapEntryType(Logic.Objects.EntryType logicEntryType)
        {
            Data.Entities.EntryType contextEntryType = new Entities.EntryType()
            {
                EntryTypeId = logicEntryType.entryTypeID,
                EntryTypeName = logicEntryType.entryTypeName,
                EntryTypeDescription = logicEntryType.entryTypeDescription,
                AdminOnly = logicEntryType.adminOnly
            };
            return contextEntryType;
        }
        #endregion
    }
}