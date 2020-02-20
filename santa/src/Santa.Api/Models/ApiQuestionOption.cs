using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models
{
    public class ApiQuestionOption
    {
        public Guid surveyOptionID { get; set; }
        public string displayText { get; set; }
        public string surveyOptionValue { get; set; }
        public string sortOrder { get; set; }
        public bool isActive { get; set; }
    }
}
