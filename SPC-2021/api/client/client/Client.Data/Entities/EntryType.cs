using System;
using System.Collections.Generic;

#nullable disable

namespace Client.Data.Entities
{
    public partial class EntryType
    {
        public EntryType()
        {
            BoardEntries = new HashSet<BoardEntry>();
        }

        public Guid EntryTypeId { get; set; }
        public string EntryTypeName { get; set; }
        public string EntryTypeDescription { get; set; }
        public bool AdminOnly { get; set; }

        public virtual ICollection<BoardEntry> BoardEntries { get; set; }
    }
}
