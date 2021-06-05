using System;
using System.Collections.Generic;

namespace Client.Logic.Client_Models
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
