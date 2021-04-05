using System;
using System.Collections.Generic;

#nullable disable

namespace Client.Data.Entities
{
    public partial class Category
    {
        public Category()
        {
            YuleLogs = new HashSet<YuleLog>();
        }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public virtual ICollection<YuleLog> YuleLogs { get; set; }
    }
}
