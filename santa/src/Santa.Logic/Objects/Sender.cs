using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Sender
    {
        public Guid senderClientID { get; set; }
        public string senderName { get; set; }
        public Guid senderEvent { get; set; }
    }
}
