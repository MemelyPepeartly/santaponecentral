using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Santa.Data.Entities;
using Santa.Logic.Constants;
using Santa.Logic.Objects;
using Santa.Logic.Objects.Base_Objects.Logging;
using Santa.Logic.Objects.Information_Objects;

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
                assignments = contextClient.ClientRelationXrefSenderClient.Count > 0 ? contextClient.ClientRelationXrefSenderClient.Select(x => Mapper.MapRelationshipMeta(x, x.RecipientClientId)).ToList() : new List<RelationshipMeta>(),
                senders = contextClient.ClientRelationXrefRecipientClient.Count > 0 ? contextClient.ClientRelationXrefRecipientClient.Select(x => Mapper.MapRelationshipMeta(x, x.SenderClientId)).ToList() : new List<RelationshipMeta>(),
                tags = contextClient.ClientTagXref.Select(Mapper.MapTagRelationXref).OrderBy(t => t.tagName).ToList(),
                notes = contextClient.Note.Select(Mapper.MapNote).ToList()
            };

            return logicClient;
        }
        public static StrippedClient MapStrippedClient(Entities.Client contextClient)
        {
            StrippedClient logicStrippedClient = new StrippedClient()
            {
                clientID = contextClient.ClientId,
                email = contextClient.Email,
                nickname = contextClient.Nickname,
                clientName = contextClient.ClientName,
                isAdmin = contextClient.IsAdmin,

                clientStatus = Mapper.MapStatus(contextClient.ClientStatus),
                responses = contextClient.SurveyResponse.Select(Mapper.MapResponse).ToList(),
                tags = contextClient.ClientTagXref.Select(Mapper.MapTagRelationXref).OrderBy(t => t.tagName).ToList()
            };

            return logicStrippedClient;
        }
        /// <summary>
        /// Maps a context relationship to a relationship meta
        /// </summary>
        /// <param name="contextRecipientXref"></param>
        /// <returns></returns>
        public static Logic.Objects.RelationshipMeta MapRelationshipMeta(Data.Entities.ClientRelationXref contextXrefRelationship, Guid clientIDMetaToMap)
        {
            List<ClientTagXref> tagXrefList = contextXrefRelationship.SenderClientId != clientIDMetaToMap ? contextXrefRelationship.RecipientClient.ClientTagXref.ToList() : contextXrefRelationship.SenderClient.ClientTagXref.ToList();
            ClientMeta logicMeta = contextXrefRelationship.SenderClientId != clientIDMetaToMap ? Mapper.MapClientMeta(contextXrefRelationship.RecipientClient) : Mapper.MapClientMeta(contextXrefRelationship.SenderClient);

            Logic.Objects.RelationshipMeta logicRelationship = new RelationshipMeta()
            {
                relationshipClient = logicMeta,
                eventType = Mapper.MapEvent(contextXrefRelationship.EventType),
                clientRelationXrefID = contextXrefRelationship.ClientRelationXrefId,
                tags = tagXrefList.Select(Mapper.MapTagRelationXref).OrderBy(t => t.tagName).ToList(),
                assignmentStatus = MapAssignmentStatus(contextXrefRelationship.AssignmentStatus),
                removable = contextXrefRelationship.ChatMessage.Count > 0 ? false : true
            };
            return logicRelationship;
        }
        public static Logic.Objects.ProfileRecipient MapProfileRecipient(Data.Entities.ClientRelationXref contextSenderXref, Data.Entities.Client contextRecipientClientData)
        {
            Logic.Objects.ProfileRecipient logicProfileRecipient = new ProfileRecipient()
            {
                relationXrefID = contextSenderXref.ClientRelationXrefId,
                recipientClient = Mapper.MapClientMeta(contextSenderXref.RecipientClient),
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
        public static AllowedAssignmentMeta MapAllowedAssignmentMeta(Logic.Objects.Client logicClient)
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
                assignments = contextClient.ClientRelationXrefSenderClient.Select(s => Mapper.MapProfileRecipient(s, s.RecipientClient)).OrderBy(pr => pr.recipientClient.clientNickname).ToList(),
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
                surveyQuestions = contextSurvey.SurveyQuestionXref.Select(q => Mapper.MapQuestion(q.SurveyQuestion)).OrderBy(s => s.sortOrder).ToList(),
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
                sortOrder = contextSurveyQuestion.SurveyQuestionXref.FirstOrDefault(sqxr => sqxr.SurveyQuestionId == contextSurveyQuestion.SurveyQuestionId).SortOrder,
                senderCanView = contextSurveyQuestion.SenderCanView,
                surveyOptionList = contextSurveyQuestion.SurveyQuestionOptionXref.Select(Mapper.MapSurveyQuestionOption).OrderBy(o => o.sortOrder).ToList(),
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

        #region Message History
        /// <summary>
        /// Maps the information of a message history with the contents of a relationship, and the viewer subject client information
        /// </summary>
        /// <param name="contextRelationshipXref"></param>
        /// <param name="logicSubjectClient"></param>
        /// <returns></returns>
        public static Logic.Objects.MessageHistory MapHistoryInformation(ClientRelationXref contextRelationshipXref, Logic.Objects.Client logicSubjectClient)
        {
            List<Message> logicListRecieverMessages = new List<Message>();
            List<Message> logicListSubjectMessages = new List<Message>();

            if (logicSubjectClient.isAdmin)
            {
                logicListSubjectMessages = contextRelationshipXref.ChatMessage
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => m.fromAdmin)
                    .ToList();

                logicListRecieverMessages = contextRelationshipXref.ChatMessage
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => !m.fromAdmin)
                    .ToList();
            }
            else
            {
                logicListSubjectMessages = contextRelationshipXref.ChatMessage
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => !m.fromAdmin && m.senderClient.clientId == logicSubjectClient.clientID)
                    .ToList();

                logicListRecieverMessages = contextRelationshipXref.ChatMessage
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => m.fromAdmin)
                    .ToList();
            }



            MessageHistory logicHistory = new MessageHistory()
            {
                relationXrefID = contextRelationshipXref.ClientRelationXrefId,
                eventType = MapEvent(contextRelationshipXref.EventType),
                assignmentStatus = MapAssignmentStatus(contextRelationshipXref.AssignmentStatus),

                subjectClient = MapClientChatMeta(logicSubjectClient),
                conversationClient = MapClientChatMeta(contextRelationshipXref.SenderClient),
                assignmentRecieverClient = MapClientChatMeta(contextRelationshipXref.RecipientClient),
                assignmentSenderClient = MapClientChatMeta(contextRelationshipXref.SenderClient),

                subjectMessages = logicListSubjectMessages,

                recieverMessages = logicListRecieverMessages,

                unreadCount = logicListRecieverMessages.Where(m => m.isMessageRead == false).ToList().Count()
            };

            foreach (Message logicMessage in logicHistory.subjectMessages)
            {
                logicMessage.subjectMessage = true;
            }

            return logicHistory;
        }
        /// <summary>
        /// Maps history information for general chats, which do not have a relationshipXrefID, EventType, or AssignmentClient using a list of contextChatMessages relating to the client's general conversation
        /// </summary>
        /// <param name="contextConversationClient"></param>
        /// <param name="contextChatMessages"></param>
        /// <param name="logicSubjectClient"></param>
        /// <returns></returns>
        public static Logic.Objects.MessageHistory MapHistoryInformation(Entities.Client contextConversationClient, List<Entities.ChatMessage> contextChatMessages, Logic.Objects.Client logicSubjectClient)
        {
            List<Message> logicListRecieverMessages = new List<Message>();
            List<Message> logicListSubjectMessages = new List<Message>();

            if (logicSubjectClient.isAdmin)
            {
                logicListSubjectMessages = contextChatMessages
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => m.fromAdmin)
                    .ToList();

                logicListRecieverMessages = contextChatMessages
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => !m.fromAdmin)
                    .ToList();
            }
            else
            {
                logicListSubjectMessages = contextChatMessages
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => !m.fromAdmin && m.senderClient.clientId == logicSubjectClient.clientID)
                    .ToList();

                logicListRecieverMessages = contextChatMessages
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => m.fromAdmin)
                    .ToList();
            }

            // General histories dont have a relationXrefID, EventType, or AssignmentClient because they are not tied to an assignment
            MessageHistory logicHistory = new MessageHistory()
            {
                relationXrefID = null,
                eventType = new Event(),
                assignmentStatus = new Logic.Objects.AssignmentStatus(),

                subjectClient = MapClientChatMeta(logicSubjectClient),
                conversationClient = MapClientChatMeta(contextConversationClient),
                assignmentRecieverClient = new ClientChatMeta(),
                assignmentSenderClient = new ClientChatMeta(),

                subjectMessages = logicListSubjectMessages,

                recieverMessages = logicListRecieverMessages,

                unreadCount = logicListRecieverMessages.Where(m => m.isMessageRead == false).ToList().Count()
            };

            logicHistory.unreadCount = logicHistory.recieverMessages.Where(m => m.isMessageRead == false).ToList().Count();

            foreach (Message logicMessage in logicHistory.subjectMessages)
            {
                logicMessage.subjectMessage = true;
            }

            return logicHistory;
        }

        /// <summary>
        /// Maps history information for general chats, which do not have a relationshipXrefID, EventType, or AssignmentClient using a list of contextChatMessages relating to the client's general conversation. Uses a logic client object
        /// rather than a context client object
        /// </summary>
        /// <param name="logicConversationClient"></param>
        /// <param name="contextChatMessages"></param>
        /// <param name="logicSubjectClient"></param>
        /// <returns></returns>
        public static Logic.Objects.MessageHistory MapHistoryInformation(Logic.Objects.Client logicConversationClient, List<Entities.ChatMessage> contextChatMessages, Logic.Objects.Client logicSubjectClient)
        {
            List<Message> logicListRecieverMessages = new List<Message>();
            List<Message> logicListSubjectMessages = new List<Message>();

            if (logicSubjectClient.isAdmin)
            {
                logicListSubjectMessages = contextChatMessages
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => m.fromAdmin)
                    .ToList();

                logicListRecieverMessages = contextChatMessages
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => !m.fromAdmin)
                    .ToList();
            }
            else
            {
                logicListSubjectMessages = contextChatMessages
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => !m.fromAdmin && m.senderClient.clientId == logicSubjectClient.clientID)
                    .ToList();

                logicListRecieverMessages = contextChatMessages
                    .Select(Mapper.MapMessage)
                    .OrderBy(dt => dt.dateTimeSent)
                    .Where(m => m.fromAdmin)
                    .ToList();
            }

            // General histories dont have a relationXrefID, EventType, or AssignmentClient because they are not tied to an assignment
            MessageHistory logicHistory = new MessageHistory()
            {
                relationXrefID = null,
                eventType = new Event(),
                assignmentStatus = new Logic.Objects.AssignmentStatus(),

                subjectClient = MapClientChatMeta(logicSubjectClient),
                conversationClient = MapClientChatMeta(logicConversationClient),
                assignmentRecieverClient = new ClientChatMeta(),
                assignmentSenderClient = new ClientChatMeta(),

                subjectMessages = logicListSubjectMessages,

                recieverMessages = logicListRecieverMessages,

                unreadCount = logicListRecieverMessages.Where(m => m.isMessageRead == false).ToList().Count()
            };

            logicHistory.unreadCount = logicHistory.recieverMessages.Where(m => m.isMessageRead == false).ToList().Count();

            foreach (Message logicMessage in logicHistory.subjectMessages)
            {
                logicMessage.subjectMessage = true;
            }

            return logicHistory;
        }
        #endregion

        #region Category
        public static Logic.Objects.Base_Objects.Logging.Category MapCategory(Entities.Category contextCategory)
        {
            Logic.Objects.Base_Objects.Logging.Category logicCategory = new Logic.Objects.Base_Objects.Logging.Category()
            {
                categoryID = contextCategory.CategoryId,
                categoryName = contextCategory.CategoryName,
                categoryDescription = contextCategory.CategoryDescription
            };
            return logicCategory;
        }
        public static Entities.Category MapCategory(Logic.Objects.Base_Objects.Logging.Category logicCategory)
        {
            Entities.Category contextCategory = new Entities.Category()
            {
                CategoryId = logicCategory.categoryID,
                CategoryName = logicCategory.categoryName,
                CategoryDescription = logicCategory.categoryDescription
            };
            return contextCategory;
        }
        #endregion
        #region Yule log
        public static Logic.Objects.Base_Objects.Logging.YuleLog MapLog(Entities.YuleLog contextLog)
        {
            Logic.Objects.Base_Objects.Logging.YuleLog logicLog = new Logic.Objects.Base_Objects.Logging.YuleLog()
            {
                logID = contextLog.LogId,
                category = MapCategory(contextLog.CategoryNavigation),
                logDate = contextLog.LogDate, 
                logtext = contextLog.LogText
            };
            return logicLog;
        }
        public static Entities.YuleLog MapLog(Logic.Objects.Base_Objects.Logging.YuleLog logicLog)
        {
            Entities.YuleLog contextLog = new Entities.YuleLog()
            {
                LogId = logicLog.logID,
                Category = logicLog.category.categoryID,
                LogDate = logicLog.logDate, 
                LogText = logicLog.logtext
            };
            return contextLog;
        }
        #endregion
    }
}