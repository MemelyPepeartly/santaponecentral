using System;
using System.Collections.Generic;

#nullable disable

namespace Event.Data.Entities
{
    public partial class Client
    {
        public Client()
        {
            Notes = new HashSet<Note>();
        }

        public Guid ClientId { get; set; }
        public Guid ClientStatusId { get; set; }
        public string ClientName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsAdmin { get; set; }
        public bool HasAccount { get; set; }

        public virtual ClientStatus ClientStatus { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
