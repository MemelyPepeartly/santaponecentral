using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Search_Models
{
    public class ClientFilterModel
    {
        public List<Guid> tagQueries { get; set; }
        public List<Guid> statusQueries { get; set; }
        public List<string> nameQueries { get; set; }
        public List<string> nicknameQueries { get; set; }
        public List<int> senderCountQueries { get; set; }
        public List<int> assignmentCountQueries { get; set; }
    }
}
