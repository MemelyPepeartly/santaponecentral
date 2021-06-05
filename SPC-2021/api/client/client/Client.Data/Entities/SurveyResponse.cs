using System;

#nullable disable

namespace Client.Data.Entities
{
    public partial class SurveyResponse
    {
        public Guid SurveyResponseId { get; set; }
        public Guid SurveyId { get; set; }
        public Guid ClientId { get; set; }
        public Guid SurveyQuestionId { get; set; }
        public Guid? SurveyOptionId { get; set; }
        public string ResponseText { get; set; }

        public virtual Client Client { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual SurveyOption SurveyOption { get; set; }
        public virtual SurveyQuestion SurveyQuestion { get; set; }
    }
}
