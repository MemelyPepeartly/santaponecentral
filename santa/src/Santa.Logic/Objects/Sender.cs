using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Sender
    {
        public Guid senderClientID { get; set; }
        public Guid senderEventTypeID { get; set; }
        public AssignmentStatus assignmentStatus { get; set; }
        public bool completed { get; set; }
        public bool removable { get; set; }
    }
}
