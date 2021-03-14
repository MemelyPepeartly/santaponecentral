using System;
using System.Collections.Generic;

namespace Santa.Data.Entities
{
    public partial class Category
    {
        public Category()
        {
            YuleLog = new HashSet<YuleLog>();
        }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public virtual ICollection<YuleLog> YuleLog { get; set; }
    }
}
