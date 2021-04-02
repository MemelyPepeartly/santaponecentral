using System;
using System.Collections.Generic;

#nullable disable

namespace Profile.Data.Entities
{
    public partial class AssignmentStatus
    {
        public AssignmentStatus()
        {
            ClientRelationXrefs = new HashSet<ClientRelationXref>();
        }

        public Guid AssignmentStatusId { get; set; }
        public string AssignmentStatusName { get; set; }
        public string AssignmentStatusDescription { get; set; }

        public virtual ICollection<ClientRelationXref> ClientRelationXrefs { get; set; }
    }
}
