using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class YuleLog
    {
        public Guid LogId { get; set; }
        public DateTime LogDate { get; set; }
        public Guid Category { get; set; }
        public string LogText { get; set; }

        public virtual Category CategoryNavigation { get; set; }
    }
}
