using System;
using System.Collections.Generic;

#nullable disable

namespace Search.Data.Entities
{
    public partial class SurveyOption
    {
        public SurveyOption()
        {
            SurveyQuestionOptionXrefs = new HashSet<SurveyQuestionOptionXref>();
            SurveyResponses = new HashSet<SurveyResponse>();
        }

        public Guid SurveyOptionId { get; set; }
        public string DisplayText { get; set; }
        public string SurveyOptionValue { get; set; }

        public virtual ICollection<SurveyQuestionOptionXref> SurveyQuestionOptionXrefs { get; set; }
        public virtual ICollection<SurveyResponse> SurveyResponses { get; set; }
    }
}
