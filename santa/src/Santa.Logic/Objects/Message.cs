using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Message
    {
        public Guid chatMessageID { get; set; }
        public Guid? messageSenderClientID { get; set; }
        public Guid? messageRecieverClientID { get; set; }
        public Guid? clientRelationXrefID { get; set; }
        public bool isMessageRead { get; set; }
        public string messageContent { get; set; }
    }
}
