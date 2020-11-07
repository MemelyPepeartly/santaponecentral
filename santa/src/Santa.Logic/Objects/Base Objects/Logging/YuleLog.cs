using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects.Base_Objects.Logging
{
    public class YuleLog
    {
        public Guid logID { get; set; }
        public Category category { get; set; }
        public DateTime logDate { get; set; }
        public string logtext { get; set; }
    }
}
