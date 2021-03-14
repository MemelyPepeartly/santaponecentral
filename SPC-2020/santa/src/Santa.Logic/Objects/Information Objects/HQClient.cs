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
        /* Santahere: Might not need this container */
        public InfoContainer infoContainer { get; set; }
        /// <summary>
        /// List of survey ID's the client has answered for
        /// </summary>
        public List<Guid> answeredSurveys { get; set; }
        /// <summary>
        /// Int number of assignments
        /// </summary>
        public int assignments { get; set; }
        /// <summary>
        /// Int number of senders
        /// </summary>
        public int senders { get; set; }
        /// <summary>
        /// Int number of notes
        /// </summary>
        public int notes { get; set; }
        public List<Tag> tags { get; set; }
        public bool isAdmin { get; set; }
        /// <summary>
        /// Determines if they have an auth0 account. This is false if they are a manual signup by default
        /// </summary>
        public bool hasAccount { get; set; }
    }
}
