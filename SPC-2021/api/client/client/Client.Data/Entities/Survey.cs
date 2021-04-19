using System;
using System.Collections.Generic;

#nullable disable

namespace Client.Data.Entities
{
    public partial class Survey
    {
        public Survey()
        {
            SurveyQuestionXrefs = new HashSet<SurveyQuestionXref>();
            SurveyResponses = new HashSet<SurveyResponse>();
        }

        public Guid SurveyId { get; set; }
        public Guid EventTypeId { get; set; }
        public string SurveyDescription { get; set; }
        public bool IsActive { get; set; }

        public virtual EventType EventType { get; set; }
        public virtual ICollection<SurveyQuestionXref> SurveyQuestionXrefs { get; set; }
        public virtual ICollection<SurveyResponse> SurveyResponses { get; set; }
    }
}
