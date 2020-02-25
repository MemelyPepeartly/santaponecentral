using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Client
    {
        public Guid clientID;
        public Guid clientStatusID { get; set; }
        public string clientStatusDescription { get; set; }
        public string clientName { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
        public List<Guid> recipients { get; set; }
        public List<Guid> senders { get; set; }

        //Constructor
        public Client(string _nickname, string _email)
        {
            nickname = _nickname;
            email = _email;
        }
        public Client () { }

    }
}
