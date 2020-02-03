using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Client
    {
        public string nickname { get; set; }
        public string email { get; set; }
        public Guid clientID { get; set; }
        public List<Logic.Objects.Client> givers { get; set; } = new List<Client>();
        public List<Logic.Objects.Client> recievers { get; set; } = new List<Client>();

        //Constructor
        public Client(string _nickname, string _email)
        {
            nickname = _nickname;
            email = _email;
        }
        public Client () { }

    }
}
