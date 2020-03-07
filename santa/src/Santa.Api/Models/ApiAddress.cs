using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models
{
    public class ApiAddress
    {
        public string clientAddressLine1 { get; set; }
        public string clientAddressLine2 { get; set; }
        public string clientCity { get; set; }
        public string clientState { get; set; }
        public string clientPostalCode { get; set; }
        public string clientCountry { get; set; }
    }
}
