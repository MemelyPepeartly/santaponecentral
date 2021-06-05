using System;

namespace Client.Logic.Objects
{
    public class Response
    {
        public Guid surveyResponseID { get; set; }
        public Guid surveyID { get; set; }
        public Guid clientID { get; set; }
        public Event responseEvent { get; set; }
        public Question surveyQuestion { get; set; }
        public Guid? surveyOptionID { get; set; }
        public string responseText { get; set; }
    }
}
