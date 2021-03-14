using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Message_Models
{
    public class ApiMessageModel
    {
        public Guid? messageSenderClientID { get; set; }
        public Guid? messageRecieverClientID { get; set; }
        public Guid? clientRelationXrefID { get; set; }
        /// <summary>
        /// Specifically with this model to minimize the amount of table downloading is needed when getting the event for the email on the post message endpoint
        /// </summary>
        public Guid? eventTypeID { get; set; }
        public string messageContent { get; set; }
        public bool fromAdmin { get; set; }
    }
}
