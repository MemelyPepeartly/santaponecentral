using System;

namespace Client.Logic.Client_Models
{
    public class EditRecipientCompletionModel
    {
        public Guid recipientID { get; set; }
        public Guid eventTypeID { get; set; }
        public bool completed { get; set; }
    }
}
