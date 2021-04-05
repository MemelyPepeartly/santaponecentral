using System;
using System.Collections.Generic;

namespace Profile.Logic.Objects
{
    public class Survey
    {
        public Guid surveyID { get; set; }
        public Guid eventTypeID { get; set; }
        public string surveyDescription { get; set; }
        public bool active { get; set; }
        public List<Question> surveyQuestions { get; set; }
        public bool removable { get; set; }

    }
}
