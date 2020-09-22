using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class SearchQueries
    {
        public List<Guid> tags { get; set; }
        public List<Guid> events { get; set; }
        public List<Guid> statuses { get; set; }
        public List<string> names { get; set; }
        public List<string> nicknames { get; set; }
        public bool isHardSearch { get; set; }
    }
}
