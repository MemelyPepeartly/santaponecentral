using System;
using System.Collections.Generic;

namespace Client.Logic.Client_Models
{
    /// <summary>
    /// Used as a model for multiple relationships posted to a client for an event
    /// </summary>
    public class AddClientRelationshipsModel
    {
        public List<Guid> assignments { get; set; }
        public Guid eventTypeID { get; set; }
        public Guid assignmentStatusID { get; set; }
    }
}
