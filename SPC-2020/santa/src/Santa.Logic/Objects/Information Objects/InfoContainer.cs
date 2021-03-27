using Santa.Logic.Objects.Base_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects.Information_Objects
{
    public class InfoContainer
    {
        public Guid agentID { get; set; }
        public List<RelationshipMeta> senders { get; set; }
        public List<RelationshipMeta> assignments { get; set; }
        public List<Note> notes { get; set; }
        public List<Response> responses { get; set; }
    }
}
