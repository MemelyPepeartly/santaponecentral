using System;
using System.Collections.Generic;

#nullable disable

namespace Profile.Data.Entities
{
    public partial class Tag
    {
        public Tag()
        {
            ClientTagXrefs = new HashSet<ClientTagXref>();
        }

        public Guid TagId { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<ClientTagXref> ClientTagXrefs { get; set; }
    }
}
