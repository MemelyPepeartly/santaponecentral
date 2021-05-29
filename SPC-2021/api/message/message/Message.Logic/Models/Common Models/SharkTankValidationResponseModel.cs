﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Logic.Models.Common_Models
{
    public class SharkTankValidationResponseModel
    {
        public bool isValid { get; set; }
        public bool isRequestSuccess { get; set; }
        public List<string> errorMessages { get; set; }
    }
}
