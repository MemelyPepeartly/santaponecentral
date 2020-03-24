using System;
using System.Collections.Generic;

namespace Santa.Logic.Objects
{
    public class Client
    {
        public Guid clientID;
        public Status clientStatus { get; set; }
        public string clientName { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
        public List<Recipient> recipients { get; set; }
        public List<Sender> senders { get; set; }

        //Constructor
        public Client(string _nickname, string _email)
        {
            nickname = _nickname;
            email = _email;
        }
        public Client () { }

    }
}
