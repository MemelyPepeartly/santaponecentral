using System;

namespace Client.Logic.Client_Models
{
    public class EditClientStatusModel
    {
        public Guid clientStatusID { get; set; }
        public bool wantsAccount { get; set; }
    }
}
