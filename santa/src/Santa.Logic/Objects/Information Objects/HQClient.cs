using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects.Information_Objects
{
    public class HQClient
    {
        public Guid clientID { get; set; }
        public Status clientStatus { get; set; }
        public string clientName { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public InfoContainer infoContainer { get; set; }
        public List<Guid> answeredSurveys { get; set; }
        public int assignments { get; set; }
        public int senders { get; set; }
        public List<Tag> tags { get; set; }
        public bool isAdmin { get; set; }
        /// <summary>
        /// Determines if they have an auth0 account. This is false if they are a manual signup by default
        /// </summary>
        public bool hasAccount { get; set; }
    }
}
