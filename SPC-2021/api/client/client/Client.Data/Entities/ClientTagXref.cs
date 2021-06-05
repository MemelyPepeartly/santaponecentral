using System;

#nullable disable

namespace Client.Data.Entities
{
    public partial class ClientTagXref
    {
        public Guid ClientTagXrefId { get; set; }
        public Guid ClientId { get; set; }
        public Guid TagId { get; set; }

        public virtual Client Client { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
