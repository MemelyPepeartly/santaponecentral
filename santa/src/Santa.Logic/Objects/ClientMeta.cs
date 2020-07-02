using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class ClientMeta
    {
        public Guid? clientId { get; set; }
        public string clientName { get; set; }
        public string clientNickname { get; set; }
    }
}
