using System.Collections.Generic;

namespace Search.Logic.Models.Common_Models
{
    public class SharkTankValidationResponseModel
    {
        public bool isValid { get; set; }
        public bool isRequestSuccess { get; set; }
        public List<string> errorMessages { get; set; }
    }
}
