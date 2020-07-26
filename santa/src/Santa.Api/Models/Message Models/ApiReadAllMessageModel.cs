using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Message_Models
{
    public class ApiReadAllMessageModel
    {
        public List<Guid> messages { get; set; }
    }
}
