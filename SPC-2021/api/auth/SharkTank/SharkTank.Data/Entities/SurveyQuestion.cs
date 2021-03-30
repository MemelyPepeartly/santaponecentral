using System;
using System.Collections.Generic;

#nullable disable

namespace SharkTank.Data.Entities
{
    public partial class SurveyQuestion
    {
        public SurveyQuestion()
        {
            SurveyQuestionOptionXrefs = new HashSet<SurveyQuestionOptionXref>();
            SurveyQuestionXrefs = new HashSet<SurveyQuestionXref>();
            SurveyResponses = new HashSet<SurveyResponse>();
        }

        public Guid SurveyQuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool SenderCanView { get; set; }
        public bool IsSurveyOptionList { get; set; }

        public virtual ICollection<SurveyQuestionOptionXref> SurveyQuestionOptionXrefs { get; set; }
        public virtual ICollection<SurveyQuestionXref> SurveyQuestionXrefs { get; set; }
        public virtual ICollection<SurveyResponse> SurveyResponses { get; set; }
    }
}
