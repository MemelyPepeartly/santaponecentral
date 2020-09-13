using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Assignment_Status_Models
{
    public class UpdateAssignmentStatusModel
    {
        public string assignmentStatusName { get; set; }
        public string assignmentStatusDescription { get; set; }
    }
}
