using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects.Information_Objects
{
    public class BaseClient
    {
        public Guid clientID { get; set; }
        public string clientName { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public bool isAdmin { get; set; }
        public bool hasAccount { get; set; }

    }
}
