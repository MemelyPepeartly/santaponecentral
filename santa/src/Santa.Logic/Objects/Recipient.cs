using System;

namespace Santa.Logic.Objects
{
    public class Recipient
    {
        public Guid recipientClientID { get; set; }
        public string recipientNickname { get; set; }
        public Guid recipientEventTypeID { get; set; }
    }
}
