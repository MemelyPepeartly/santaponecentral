using Profile.Data.Entities;
using Profile.Logic.Constants;
using Profile.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Data.Repository
{
    public static class Mapper
    {
        #region Profile
        public static Logic.Objects.Profile MapProfile(Entities.Client contextClient)
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

        #region Event

        public static Logic.Objects.Event MapEvent(Entities.EventType contextEventType)
        {
            Logic.Objects.Event logicEvent = new Logic.Objects.Event()
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

        #region Meta
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

        #endregion
    }
}
