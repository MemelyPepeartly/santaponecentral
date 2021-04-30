using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Logic.Client_Models
{
    public class EditClientStatusModel
    {
        public Guid clientStatusID { get; set; }
        public bool wantsAccount { get; set; }
    }
}
