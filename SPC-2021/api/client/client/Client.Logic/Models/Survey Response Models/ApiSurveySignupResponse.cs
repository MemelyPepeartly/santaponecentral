using System;

namespace Client.Logic.Survey_Response_Models
{
    /// <summary>
    /// Modified version of the survey response model. Used in signups since clientID's are not created until a client is pushed to the DB
    /// </summary>
    public class ApiSurveySignupResponse
    {
        public Guid surveyID { get; set; }
        public Guid surveyQuestionID { get; set; }
        public Guid? surveyOptionID { get; set; }
        public string responseText { get; set; }
    }
}
