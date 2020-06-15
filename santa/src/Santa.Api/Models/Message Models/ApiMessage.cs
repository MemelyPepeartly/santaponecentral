using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Message_Models
{
    public class ApiMessage
    {
        public Guid? messageSenderClientID { get; set; }
        public Guid? messageRecieverClientID { get; set; }
        public Guid? clientRelationXrefID { get; set; }

        public string messageContent { get; set; }
    }
}
