using Client.Data.Entities;
using Client.Logic.Constants;
using Client.Logic.Objects;
using Client.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.Data.Repository
{
    public static class Mapper
    {

        #region Client
        public static Logic.Objects.Client MapStaticClient(Entities.Client contextClient)
        {
            Logic.Objects.Client logicClient = new Logic.Objects.Client()
            {
                clientID = contextClient.ClientId,
                email = contextClient.Email,
                clientName = contextClient.ClientName,
                nickname = contextClient.Nickname,
                hasAccount = contextClient.HasAccount,
                isAdmin = contextClient.IsAdmin,
                address = new Address
                {
                    addressLineOne = contextClient.AddressLine1,
                    addressLineTwo = contextClient.AddressLine2,
                    city = contextClient.City,
                    country = contextClient.Country,
                    state = contextClient.State,
                    postalCode = contextClient.PostalCode
                }
            };
            return logicClient;
        }
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
        /// Maps a context relationship to a relationship meta
        /// </summary>
        /// <param name="contextRecipientXref"></param>
        /// <returns></returns>
        public static RelationshipMeta MapRelationshipMeta(ClientRelationXref contextXrefRelationship, Guid clientIDMetaToMap)
        {
            List<ClientTagXref> tagXrefList = contextXrefRelationship.SenderClientId != clientIDMetaToMap ? contextXrefRelationship.RecipientClient.ClientTagXrefs.ToList() : contextXrefRelationship.SenderClient.ClientTagXrefs.ToList();
            ClientMeta logicMeta = contextXrefRelationship.SenderClientId != clientIDMetaToMap ? Mapper.MapClientMeta(contextXrefRelationship.RecipientClient) : Mapper.MapClientMeta(contextXrefRelationship.SenderClient);

            Logic.Objects.RelationshipMeta logicRelationship = new RelationshipMeta()
            {
                relationshipClient = logicMeta,
                eventType = Mapper.MapEvent(contextXrefRelationship.EventType),
                clientRelationXrefID = contextXrefRelationship.ClientRelationXrefId,
                tags = tagXrefList.Select(Mapper.MapTagRelationXref).OrderBy(t => t.tagName).ToList(),
                assignmentStatus = MapAssignmentStatus(contextXrefRelationship.AssignmentStatus),
                removable = contextXrefRelationship.ChatMessages.Count > 0 ? false : true
            };
            return logicRelationship;
        }
        public static ProfileAssignment MapProfileRecipient(ClientRelationXref contextClientRelationXref)
        {
            ProfileAssignment logicProfileRecipient = new ProfileAssignment()
            {
                relationXrefID = contextClientRelationXref.ClientRelationXrefId,
                recipientClient = Mapper.MapClientMeta(contextClientRelationXref.RecipientClient),
                recipientEvent = Mapper.MapEvent(contextClientRelationXref.EventType),
                assignmentStatus = MapAssignmentStatus(contextClientRelationXref.AssignmentStatus),

                address = new Address
                {
                    addressLineOne = contextClientRelationXref.RecipientClient.AddressLine1,
                    addressLineTwo = contextClientRelationXref.RecipientClient.AddressLine2,
                    city = contextClientRelationXref.RecipientClient.City,
                    country = contextClientRelationXref.RecipientClient.Country,
                    state = contextClientRelationXref.RecipientClient.State,
                    postalCode = contextClientRelationXref.RecipientClient.PostalCode
                },
                responses = contextClientRelationXref.RecipientClient.SurveyResponses.Select(Mapper.MapResponse).ToList()
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
                hasAccount = contextClient.HasAccount,
                isAdmin = contextClient.IsAdmin
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
                hasAccount = logicClient.hasAccount,
                isAdmin = logicClient.isAdmin
            };

            return logicMeta;
        }
        public static ClientMeta MapClientMeta(HQClient logicHQClient)
        {
            Logic.Objects.ClientMeta logicMeta = new ClientMeta()
            {
                clientId = logicHQClient.clientID,
                clientName = logicHQClient.clientName,
                clientNickname = logicHQClient.nickname,
                hasAccount = logicHQClient.hasAccount,
                isAdmin = logicHQClient.isAdmin
            };

            return logicMeta;
        }
        public static ClientChatMeta MapClientChatMeta(Entities.Client contextClient)
        {
            ClientChatMeta logicMeta = new ClientChatMeta()
            {
                clientId = contextClient.ClientId,
                clientNickname = contextClient.Nickname,
                hasAccount = contextClient.HasAccount,
                isAdmin = contextClient.IsAdmin
            };

            return logicMeta;
        }
        public static ClientChatMeta MapClientChatMeta(BaseClient baseLogicClient)
        {
            ClientChatMeta logicMeta = new ClientChatMeta()
            {
                clientId = baseLogicClient.clientID,
                clientNickname = baseLogicClient.nickname,
                hasAccount = baseLogicClient.hasAccount,
                isAdmin = baseLogicClient.isAdmin
            };

            return logicMeta;
        }
        public static ClientChatMeta MapClientChatMeta(Logic.Objects.Client logicClient)
        {
            ClientChatMeta logicMeta = new ClientChatMeta()
            {
                clientId = logicClient.clientID,
                clientNickname = logicClient.nickname,
                hasAccount = logicClient.hasAccount,
                isAdmin = logicClient.isAdmin
            };

            return logicMeta;
        }

        #endregion

        #region Allowed Assignment Meta
        public static AllowedAssignmentMeta MapAllowedAssignmentMeta(Client.Logic.Objects.Client logicClient)
        {
            AllowedAssignmentMeta logicAllowedAssignmentMeta = new AllowedAssignmentMeta()
            {
                clientMeta = Mapper.MapClientMeta(logicClient),
                tags = logicClient.tags,
                totalSenders = logicClient.senders.Count,
                totalAssignments = logicClient.assignments.Count
            };

            return logicAllowedAssignmentMeta;
        }
        public static AllowedAssignmentMeta MapAllowedAssignmentMeta(HQClient logicHQClient)
        {
            AllowedAssignmentMeta logicAllowedAssignmentMeta = new AllowedAssignmentMeta()
            {
                clientMeta = Mapper.MapClientMeta(logicHQClient),
                tags = logicHQClient.tags,
                totalSenders = logicHQClient.senders,
                totalAssignments = logicHQClient.assignments
            };

            return logicAllowedAssignmentMeta;
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
                //assignments = contextClient.ClientRelationXrefSenderClient.Select(s => Mapper.MapProfileRecipient(s, s.RecipientClient)).OrderBy(pr => pr.recipientClient.clientNickname).ToList(),
                responses = contextClient.SurveyResponses.Select(Mapper.MapResponse).ToList(),
                editable = contextClient.ClientRelationXrefRecipientClients.Count > 0 ? false : true
            };

            return logicProfile;

        }
        #endregion

        #region Tag
        public static Logic.Objects.Tag MapTag(Entities.Tag contextTag)
        {
            Logic.Objects.Tag logicTag = new Logic.Objects.Tag()
            {
                tagID = contextTag.TagId,
                tagName = contextTag.TagName,
                deletable = contextTag.ClientTagXrefs.Count > 0 ? false : true,
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
                recieverClient = new ClientChatMeta()
                {
                    clientId = contextMessage.MessageReceiverClientId != null ? contextMessage.MessageReceiverClientId : null,
                    clientNickname = contextMessage.MessageReceiverClientId != null ? contextMessage.MessageReceiverClient.Nickname : String.Empty
                },
                senderClient = new ClientChatMeta()
                {
                    clientId = contextMessage.MessageSenderClientId != null ? contextMessage.MessageSenderClientId : null,
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

        public static Event MapEvent(EventType contextEventType)
        {
            Event logicEvent = new Event()
            {
                eventTypeID = contextEventType.EventTypeId,
                eventDescription = contextEventType.EventDescription,
                active = contextEventType.IsActive,
                removable = contextEventType.ClientRelationXrefs.Count == 0 && contextEventType.Surveys.Count == 0,
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
                surveyQuestions = contextSurvey.SurveyQuestionXrefs.Select(q => Mapper.MapQuestion(q.SurveyQuestion)).OrderBy(s => s.sortOrder).ToList(),
                removable = contextSurvey.SurveyQuestionXrefs.Count == 0
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
        public static SurveyMeta MapSurveyMeta(Response response)
        {
            SurveyMeta logicSurveyMeta = new SurveyMeta()
            {
                surveyID = response.surveyID,
                eventTypeID = response.responseEvent.eventTypeID
            };
            return logicSurveyMeta;
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
                sortOrder = contextSurveyQuestion.SurveyQuestionXrefs.Count != 0 ? contextSurveyQuestion.SurveyQuestionXrefs.FirstOrDefault(sqxr => sqxr.SurveyQuestionId == contextSurveyQuestion.SurveyQuestionId).SortOrder : 0,
                senderCanView = contextSurveyQuestion.SenderCanView,
                surveyOptionList = contextSurveyQuestion.SurveyQuestionOptionXrefs.Select(Mapper.MapSurveyQuestionOption).OrderBy(o => o.sortOrder).ToList(),
                removable = contextSurveyQuestion.SurveyResponses.Count == 0 && contextSurveyQuestion.SurveyQuestionOptionXrefs.Count == 0
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
                removable = contextSurveyOption.SurveyResponses.Count == 0
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
                surveyOptionValue = contextQuestionOption.SurveyOption.SurveyOptionValue,
                sortOrder = contextQuestionOption.SortOrder
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

        #region Note
        public static Entities.Note MapNote(Logic.Objects.Base_Objects.Note logicNote)
        {
            Note contextNote = new Note()
            {
                NoteId = logicNote.noteID,
                NoteSubject = logicNote.noteSubject,
                NoteContents = logicNote.noteContents
            };
            return contextNote;
        }
        public static Logic.Objects.Base_Objects.Note MapNote(Entities.Note contextNote)
        {
            Logic.Objects.Base_Objects.Note logicNote = new Logic.Objects.Base_Objects.Note()
            {
                noteID = contextNote.NoteId,
                noteSubject = contextNote.NoteSubject,
                noteContents = contextNote.NoteContents
            };
            return logicNote;
        }
        #endregion
    }
}