using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class SurveyQuestion
    {
        public SurveyQuestion()
        {
            SurveyQuestionOptionXref = new HashSet<SurveyQuestionOptionXref>();
            SurveyQuestionXref = new HashSet<SurveyQuestionXref>();
            SurveyResponse = new HashSet<SurveyResponse>();
        }

        public Guid SurveyQuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool IsSurveyOptionList { get; set; }

        public virtual ICollection<SurveyQuestionOptionXref> SurveyQuestionOptionXref { get; set; }
        public virtual ICollection<SurveyQuestionXref> SurveyQuestionXref { get; set; }
        public virtual ICollection<SurveyResponse> SurveyResponse { get; set; }
    }
}
