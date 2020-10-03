using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class AssignmentStatus
    {
        public AssignmentStatus()
        {
            ClientRelationXref = new HashSet<ClientRelationXref>();
        }

        public Guid AssignmentStatusId { get; set; }
        public string AssignmentStatusName { get; set; }
        public string AssignmentStatusDescription { get; set; }

        public virtual ICollection<ClientRelationXref> ClientRelationXref { get; set; }
    }
}
