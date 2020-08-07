using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Tag
    {
        public Guid tagID { get; set; }
        public string tagName { get; set; }
        public bool deletable { get; set; }
    }
}
