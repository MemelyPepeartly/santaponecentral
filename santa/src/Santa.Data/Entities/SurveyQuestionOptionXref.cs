using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class SurveyQuestionOptionXref
    {
        public Guid SurveyQuestionId { get; set; }
        public Guid SurveyOptionId { get; set; }
        public string SortOrder { get; set; }
        public bool IsAction { get; set; }

        public virtual SurveyOption SurveyOption { get; set; }
        public virtual SurveyQuestion SurveyQuestion { get; set; }
    }
}
