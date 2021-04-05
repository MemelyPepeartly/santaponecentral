using System;
using System.Collections.Generic;
using System.Text;

namespace SharkTank.Logic.Objects
{
    public class ClientMeta
    {
        public Guid? clientId { get; set; }
        public string clientName { get; set; }
        public string clientNickname { get; set; }
        public bool hasAccount { get; set; }
        public bool isAdmin { get; set; }
    }
}
