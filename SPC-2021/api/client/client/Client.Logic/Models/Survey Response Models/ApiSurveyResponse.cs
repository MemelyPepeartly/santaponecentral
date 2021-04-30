using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharkTank.Logic.Survey_Response_Models
{
    public class ApiSurveyResponse
    {
        public Guid surveyID { get; set; }
        public Guid clientID { get; set; }
        public Guid surveyQuestionID { get; set; }
        public Guid? surveyOptionID { get; set; }
        public string responseText { get; set; }
    }
}
