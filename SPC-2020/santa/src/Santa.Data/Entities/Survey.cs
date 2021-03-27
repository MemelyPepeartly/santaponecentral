using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class Survey
    {
        public Survey()
        {
            SurveyQuestionXref = new HashSet<SurveyQuestionXref>();
            SurveyResponse = new HashSet<SurveyResponse>();
        }

        public Guid SurveyId { get; set; }
        public Guid EventTypeId { get; set; }
        public string SurveyDescription { get; set; }
        public bool IsActive { get; set; }

        public virtual EventType EventType { get; set; }
        public virtual ICollection<SurveyQuestionXref> SurveyQuestionXref { get; set; }
        public virtual ICollection<SurveyResponse> SurveyResponse { get; set; }
    }
}
