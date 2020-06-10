using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.UserProfile
{
    public class UserProfile
    {
        //public string sub { get; set; }
        public string name { get; set; }
        //public string given_name { get; set; }
        //public string family_name { get; set; }
        //public string middle_name { get; set; }
        //public string nickname { get; set; }
        //public string preferred_username { get; set; }
        //public string profile { get; set; }
        //public string picture { get; set; }
        //public string website { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        //public string gender { get; set; }
        //public string birthdate { get; set; }
        //public string zoneinfo { get; set; }
        //public string locale { get; set; }
        //public string phone_number { get; set; }
        //public bool phone_number_verified { get; set; }
        //public string updated_at { get; set; }
    }
}
