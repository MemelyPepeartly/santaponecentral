using System;

namespace Santa.Logic.Objects
{
    public class Option
    {
        public Option(Guid _questionID)
        {
            questionID = _questionID;
        }
        public Guid questionID { get; set; }
        public Guid surveyOptionID { get; set; }
        public string displayText { get; set; }
        public string surveyOptionValue { get; set; }
        public string sortOrder { get; set; }
        public bool isActive { get; set; }
    }
}
