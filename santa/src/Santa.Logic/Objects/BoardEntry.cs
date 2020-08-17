﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    /// <summary>
    /// Logic object for a board entry in the database
    /// </summary>
    public class BoardEntry
    {
        public Guid boardEntryID { get; set; }
        public int postNumber { get; set; }
        public string postDescription { get; set; }
    }
}
