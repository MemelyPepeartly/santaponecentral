using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Client_Models
{
    /// <summary>
    /// Used as a model for multiple relationships posted to a client for an event
    /// </summary>
    public class ApiClientRelationshipsModel
    {
        public List<Guid> assignments { get; set; }
        public Guid eventTypeID { get; set; }
    }
}
