using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Logic.Constants
{
    public static class Constants
    {
        // Statuses
        public const string AWAITING_STATUS = "Awaiting";
        public const string APPROVED_STATUS = "Approved";
        public const string DENIED_STATUS = "Denied";
        public const string COMPLETED_STATUS = "Completed";

        // Auth0 role descriptions
        public const string PARTICIPANT = "Participant";
        public const string EVENT_ADMIN = "Event Admin";
        public const string SANTADEV = "SantaDev";

        // Default events for the secret santa events
        public const string GIFT_EXCHANGE_EVENT = "Gift Exchange";
        public const string CARD_EXCHANGE_EVENT = "Card Exchange";

        // Default tags for clients
        public const string GRINCH_TAG = "Grinch";
        public const string MASS_MAILER_TAG = "Mass Mailer";
        public const string MASS_MAIL_RECIPIENT_TAG = "Mass Mail Recipient";

        // Entry types

    }
}
