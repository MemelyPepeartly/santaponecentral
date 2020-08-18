using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class BoardEntry
    {
        public Guid BoardEntryId { get; set; }
        public Guid? EntryTypeId { get; set; }
        public int PostNumber { get; set; }
        public string PostDescription { get; set; }

        public virtual EntryType EntryType { get; set; }
    }
}
