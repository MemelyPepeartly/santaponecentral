using System;
using System.Collections.Generic;
using System.Text;

namespace Profile.Logic.Objects
{
    public class SearchQueries
    {
        public List<Guid> tags { get; set; }
        public List<Guid> events { get; set; }
        public List<Guid> statuses { get; set; }
        public List<string> names { get; set; }
        public List<string> nicknames { get; set; }
        public List<string> emails { get; set; }
        public List<string> responses { get; set; }
        public List<int> cardAssignments { get; set; }
        public List<int> giftAssignments { get; set; }
        public bool isHardSearch { get; set; }
    }
}
