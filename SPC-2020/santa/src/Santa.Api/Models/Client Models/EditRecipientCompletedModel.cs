using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Client_Models
{
    public class EditRecipientCompletionModel
    {
        public Guid recipientID { get; set; }
        public Guid eventTypeID { get; set; }
        public bool completed { get; set; }
    }
}
