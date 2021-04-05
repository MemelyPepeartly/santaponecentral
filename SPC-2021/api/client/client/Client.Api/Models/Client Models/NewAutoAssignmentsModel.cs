using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Client_Models
{
    public class Pairing
    {
        public Guid senderAgentID { get; set; }
        public Guid assignmentClientID { get; set; }
    }
    public class NewAutoAssignmentsModel
    {
        public List<Pairing> pairings { get; set; }
    }
}
