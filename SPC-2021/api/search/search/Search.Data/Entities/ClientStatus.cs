using System;
using System.Collections.Generic;

#nullable disable

namespace Search.Data.Entities
{
    public partial class ClientStatus
    {
        public ClientStatus()
        {
            Clients = new HashSet<Client>();
        }

        public Guid ClientStatusId { get; set; }
        public string StatusDescription { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
