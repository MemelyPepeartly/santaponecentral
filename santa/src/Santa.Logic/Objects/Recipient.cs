using System;

namespace Santa.Logic.Objects
{
    public class Recipient
    {
        public Guid recipientClientID { get; set; }
        public Guid recipientEventTypeID { get; set; }
        public bool completed { get; set; }
        public bool removable { get; set; }
    }
}
