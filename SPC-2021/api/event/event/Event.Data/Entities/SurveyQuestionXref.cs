using System;
using System.Collections.Generic;

#nullable disable

namespace Event.Data.Entities
{
    public partial class SurveyQuestionXref
    {
        public Guid SurveyQuestionXrefId { get; set; }
        public Guid SurveyId { get; set; }
        public Guid SurveyQuestionId { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual SurveyQuestion SurveyQuestion { get; set; }
    }
}
