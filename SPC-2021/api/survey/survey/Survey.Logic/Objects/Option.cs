using System;

namespace Survey.Logic.Objects
{
    public class Option
    {
        public Guid surveyOptionID { get; set; }
        public string displayText { get; set; }
        public int sortOrder { get; set; }
        public string surveyOptionValue { get; set; }
        public bool removable { get; set; }
    }
}
