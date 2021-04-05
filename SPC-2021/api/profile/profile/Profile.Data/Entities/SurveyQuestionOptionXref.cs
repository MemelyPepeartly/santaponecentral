using System;
using System.Collections.Generic;

#nullable disable

namespace Profile.Data.Entities
{
    public partial class SurveyQuestionOptionXref
    {
        public Guid SurveyQuestionOptionXrefId { get; set; }
        public Guid SurveyQuestionId { get; set; }
        public Guid SurveyOptionId { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }

        public virtual SurveyOption SurveyOption { get; set; }
        public virtual SurveyQuestion SurveyQuestion { get; set; }
    }
}
