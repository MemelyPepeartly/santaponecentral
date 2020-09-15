using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class RelationshipMeta
    {
        public ClientMeta relationshipClient { get; set; }
        public Guid relationshipEventTypeID { get; set; }
        public Guid clientRelationXrefID { get; set; }
        public AssignmentStatus assignmentStatus { get; set; }
        public bool completed { get; set; }
        public bool removable { get; set; }
    }
}
