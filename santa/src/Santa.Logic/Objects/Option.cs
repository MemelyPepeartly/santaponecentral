using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Option
    {
        public Guid surveyOptionID { get; set; }
        public string displayText { get; set; }
        public string surveyOptionValue { get; set; }
    }
}
