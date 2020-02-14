using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class Client
    {
        private Guid _clientID;
        public Guid clientStatusID { get; set; }
        public string clientName { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public Guid clientID
        {
            get => _clientID;
            set
            {
                if (value == Guid.Empty)
                {
                    throw new ArgumentException("Client ID cannot be empty");
                }

                _clientID = value;
            }
        }
        public Address address { get; set; }
        public List<Logic.Objects.Client> senders { get; set; } = new List<Client>();
        public List<Logic.Objects.Client> recipients { get; set; } = new List<Client>();

        //Constructor
        public Client(string _nickname, string _email)
        {
            nickname = _nickname;
            email = _email;
        }
        public Client () { }

    }
}
