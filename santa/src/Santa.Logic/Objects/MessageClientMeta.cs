using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class MessageClientMeta
    {
        public Guid? clientId { get; set; }
        public string clientName { get; set; }
        public string clientNickname { get; set; }
    }
}
