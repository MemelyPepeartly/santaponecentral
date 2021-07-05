using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Survey.Api.Authorization
{
    public class PermissionClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
