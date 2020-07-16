using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class SurveyQuestionOptionXref
    {
        public int SurveyQuestionOptionXref1 { get; set; }
        public Guid SurveyQuestionId { get; set; }
        public Guid SurveyOptionId { get; set; }
        public string SortOrder { get; set; }
        public bool IsActive { get; set; }

        public virtual SurveyOption SurveyOption { get; set; }
        public virtual SurveyQuestion SurveyQuestion { get; set; }
    }
}
