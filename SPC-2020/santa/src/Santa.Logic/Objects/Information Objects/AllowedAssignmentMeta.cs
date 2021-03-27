using Santa.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class AllowedAssignmentMeta
    {
        public ClientMeta clientMeta { get; set; }
        public List<SurveyMeta> answeredSurveys { get; set; }
        public List<Tag> tags { get; set; }
        public int totalSenders { get; set; }
        public int totalAssignments { get; set; }
    }
}
