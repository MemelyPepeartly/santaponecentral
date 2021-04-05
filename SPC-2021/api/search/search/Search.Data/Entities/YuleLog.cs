using System;
using System.Collections.Generic;

#nullable disable

namespace Search.Data.Entities
{
    public partial class YuleLog
    {
        public Guid LogId { get; set; }
        public DateTime LogDate { get; set; }
        public Guid CategoryId { get; set; }
        public string LogText { get; set; }

        public virtual Category Category { get; set; }
    }
}
