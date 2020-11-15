using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects.Information_Objects
{
    /// <summary>
    /// Stripped client does not have an address, assignments, senders, or auth0 account status info
    /// </summary>
    public class StrippedClient
    {
        public Guid clientID { get; set; }
        public Status clientStatus { get; set; }
        public string clientName { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public List<Response> responses { get; set; }
        public List<Tag> tags { get; set; }
        public int giftAssignments { get; set; }
        public int giftSenders { get; set; }
        public int cardAssignments { get; set; }
        public int cardSenders { get; set; }
        public bool isAdmin { get; set; }
    }
}
