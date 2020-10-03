using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects
{
    public class ProfileRecipient
    {
        //Xref ID needed for extra data for chat functionality
        public Guid? relationXrefID { get; set; }
        public ClientMeta recipientClient { get; set; }
        public AssignmentStatus assignmentStatus { get; set; }
        public Address address { get; set; }
        public Event recipientEvent { get; set; }
        public List<Response> responses { get; set; }

    }
}
