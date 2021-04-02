using System;
using System.Collections.Generic;
using System.Text;

namespace Message.Logic.Objects
{
    /// <summary>
    /// Logic object for a board entry in the database
    /// </summary>
    public class BoardEntry
    {
        public Guid boardEntryID { get; set; }
        public EntryType entryType { get; set; }
        public DateTime dateTimeEntered { get; set; }
        public int threadNumber { get; set; }
        public int postNumber { get; set; }
        public string postDescription { get; set; }
    }
}
