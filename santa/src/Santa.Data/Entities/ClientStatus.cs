using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class ClientStatus
    {
        public ClientStatus()
        {
            Client = new HashSet<Client>();
        }

        public Guid ClientStatusId { get; set; }
        public string StatusDescription { get; set; }

        public virtual ICollection<Client> Client { get; set; }
    }
}
