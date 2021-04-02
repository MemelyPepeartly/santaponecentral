using System;
using System.Collections.Generic;
using System.Text;

namespace Profile.Logic.Objects.Base_Objects.Logging
{
    public class YuleLog
    {
        public Guid logID { get; set; }
        public Category category { get; set; }
        public DateTime logDate { get; set; }
        public string logText { get; set; }
    }
}
