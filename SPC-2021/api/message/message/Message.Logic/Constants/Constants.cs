using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Logic.Constants
{
    public static class Constants
    {
        // Default events for the secret santa events
        public const string GIFT_EXCHANGE_EVENT = "Gift Exchange";
        public const string CARD_EXCHANGE_EVENT = "Card Exchange";

        // Statuses
        public const string AWAITING_STATUS = "Awaiting";
        public const string APPROVED_STATUS = "Approved";
        public const string DENIED_STATUS = "Denied";
        public const string COMPLETED_STATUS = "Completed";
    }
}
