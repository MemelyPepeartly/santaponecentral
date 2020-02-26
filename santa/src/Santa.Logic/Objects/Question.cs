using System;
using System.Collections.Generic;
namespace Santa.Logic.Objects
{
    /// <summary>
    /// Question that can be within a survey
    /// </summary>
    public class Question
    {
        public Question(Guid _surveyID)
        {
            surveyID = _surveyID;
        }

        public Guid questionID { get; set; }
        public Guid surveyID { get; set; }
        public string sortOrder { get; set; }
        public bool isActive { get; set; }
        public string questionText { get; set; }
        public bool isSurveyOptionList { get; set; }
        public List<Option> surveyOptionList { get; set; }
    }
}
