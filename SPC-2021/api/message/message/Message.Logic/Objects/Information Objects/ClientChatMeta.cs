using System;
using System.Collections.Generic;
using System.Text;

namespace Message.Logic.Objects.Information_Objects
{
    public class ClientChatMeta
    {
        public Guid? clientId { get; set; }
        public string clientNickname { get; set; }
        public bool hasAccount { get; set; }
        public bool isAdmin { get; set; }
    }
}
