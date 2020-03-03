using System;

namespace Santa.Api.Models
{
    public class ApiClient
    {
        public string clientName { get; set; }
        public string clientEmail { get; set; }
        public string clientNickname { get; set; }
        public Guid clientStatusID { get; set; }
        public Guid clientID { get; set; }
        public string clientAddressLine1 { get; set; }
        public string clientAddressLine2 { get; set; }
        public string clientCity { get; set; }
        public string clientState { get; set; }
        public string clientPostalCode { get; set; }
        public string clientCountry { get; set; }
    }
}
