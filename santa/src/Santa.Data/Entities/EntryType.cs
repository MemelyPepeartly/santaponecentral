using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class EntryType
    {
        public EntryType()
        {
            BoardEntry = new HashSet<BoardEntry>();
        }

        public Guid EntryTypeId { get; set; }
        public string EntryTypeName { get; set; }
        public string EntryTypeDescription { get; set; }
        public bool AdminOnly { get; set; }

        public virtual ICollection<BoardEntry> BoardEntry { get; set; }
    }
}
