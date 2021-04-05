using System;
using System.Collections.Generic;

namespace Survey.Logic.Objects
{
    /// <summary>
    /// Question that can be within a survey
    /// </summary>
    public class Question
    {
        public Guid questionID { get; set; }
        public string questionText { get; set; }
        public bool isSurveyOptionList { get; set; }
        public int sortOrder { get; set; }
        public bool senderCanView { get; set; }
        public List<Option> surveyOptionList { get; set; }
        public bool removable { get; set; }
    }
}
