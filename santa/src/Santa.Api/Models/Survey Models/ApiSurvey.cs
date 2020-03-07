using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models
{
    public class ApiSurvey
    {
        public Guid eventTypeID { get; set; }
        public string surveyDescription { get; set; }
        public bool active { get; set; }
    }
}
