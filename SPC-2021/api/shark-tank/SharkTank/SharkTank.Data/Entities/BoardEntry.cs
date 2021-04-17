using System;
using System.Collections.Generic;

#nullable disable

namespace SharkTank.Data.Entities
{
    public partial class BoardEntry
    {
        public Guid BoardEntryId { get; set; }
        public Guid? EntryTypeId { get; set; }
        public int ThreadNumber { get; set; }
        public int PostNumber { get; set; }
        public string PostDescription { get; set; }
        public DateTime DateTimeEntered { get; set; }

        public virtual EntryType EntryType { get; set; }
    }
}
