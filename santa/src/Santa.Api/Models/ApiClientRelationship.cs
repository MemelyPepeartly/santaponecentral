using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models
{
    public class ApiClientRelationship
    {
        public Guid recieverClientID { get; set; }
        public Guid eventTypeID { get; set; }
    }
}
