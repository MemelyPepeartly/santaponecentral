using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class BoardEntry
    {
        public Guid BoardEntryId { get; set; }
        public int PostNumber { get; set; }
        public string PostDescription { get; set; }
    }
}
