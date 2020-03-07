using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models
{
    public class ApiQuestion
    {
        public string questionText { get; set; }
        public bool isSurveyOptionList { get; set; }

        public Guid surveyID { get; set; }
        public string sortOrder { get; set; }
        public bool isActive { get; set; }
    }
}
