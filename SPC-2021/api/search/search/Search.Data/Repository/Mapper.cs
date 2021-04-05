using Search.Data.Entities;
using Search.Logic.Constants;
using Search.Logic.Objects;
using Search.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.Data.Repository
{
    public static class Mapper
    {
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
        #endregion

        #region Question
        /// <summary>
        /// Maps a context question to a logic question
        /// </summary>
        /// <param name="contextSurveyQuestion"></param>
        /// <returns></returns>
        public static Question MapQuestion(SurveyQuestion contextSurveyQuestion)
        {

            Question logicQuestion = new Question()
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
        public static SurveyQuestionXref MapQuestionXref(Question logicQuestion)
        {
            SurveyQuestionXref contextQuestionXref = new SurveyQuestionXref()
            {
                SurveyQuestionId = logicQuestion.questionID,
            };
            return contextQuestionXref;
        }
        #endregion

        #region SurveyOption
        public static Option MapSurveyOption(SurveyOption contextSurveyOption)
        {
            Option logicSurveyOption = new Option()
            {
                surveyOptionID = contextSurveyOption.SurveyOptionId,
                displayText = contextSurveyOption.DisplayText,
                surveyOptionValue = contextSurveyOption.SurveyOptionValue,
                removable = contextSurveyOption.SurveyResponses.Count == 0
            };
            return logicSurveyOption;
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
        public static Option MapSurveyQuestionOption(SurveyQuestionOptionXref contextQuestionOption)
        {
            Option logicOption = new Option()
            {
                surveyOptionID = contextQuestionOption.SurveyOption.SurveyOptionId,
                displayText = contextQuestionOption.SurveyOption.DisplayText,
                surveyOptionValue = contextQuestionOption.SurveyOption.SurveyOptionValue,
                sortOrder = contextQuestionOption.SortOrder
            };
            return logicOption;
        }
        #endregion
        #endregion
    }
}
