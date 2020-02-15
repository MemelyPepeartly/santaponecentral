using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Question
    {
        public Guid questionID { get; set; }
        public string questionText { get; set; }
        public bool isSurveyOptionList { get; set; }
    }
}
