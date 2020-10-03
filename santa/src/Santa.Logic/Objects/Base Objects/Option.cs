using System;

namespace Santa.Logic.Objects
{
    public class Option
    {
        public Guid surveyOptionID { get; set; }
        public string displayText { get; set; }
        public string surveyOptionValue { get; set; }
        public bool removable { get; set; }
    }
}
