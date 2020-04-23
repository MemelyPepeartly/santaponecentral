using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class SurveyOption
    {
        public SurveyOption()
        {
            SurveyQuestionOptionXref = new HashSet<SurveyQuestionOptionXref>();
            SurveyResponse = new HashSet<SurveyResponse>();
        }

        public Guid SurveyOptionId { get; set; }
        public string DisplayText { get; set; }
        public string SurveyOptionValue { get; set; }

        public virtual ICollection<SurveyQuestionOptionXref> SurveyQuestionOptionXref { get; set; }
        public virtual ICollection<SurveyResponse> SurveyResponse { get; set; }
    }
}
