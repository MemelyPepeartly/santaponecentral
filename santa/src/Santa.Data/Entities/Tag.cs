using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class Tag
    {
        public Tag()
        {
            ClientTagXref = new HashSet<ClientTagXref>();
        }

        public Guid TagId { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<ClientTagXref> ClientTagXref { get; set; }
    }
}
