using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Catalogue_Models
{
    public class searchQueryModel
    {
        public List<Guid> tags { get; set; }
        public List<Guid> events { get; set; }
        public List<Guid> statuses { get; set; }
    }
}
