using System;

namespace Profile.Logic.Objects
{
    public class EntryType
    {
        public Guid entryTypeID { get; set; }
        public string entryTypeName { get; set; }
        public string entryTypeDescription { get; set; }
        public bool adminOnly { get; set; }
    }
}
