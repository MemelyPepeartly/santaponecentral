namespace Client.Logic.Constants
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
        public const string HELPER = "Santa's Lil Helper";
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

        public const string DEADLINE_ANNOUNCEMENTS_ENTRYTYPE = "Announcements (Deadlines)";
        public const string CONTEST_ANNOUNCEMENTS_ENTRYTYPE = "Announcements (Contests)";
        public const string GIFT_DELIVERIES_ENTRYTYPE = "Deliveries (Gifts)";
        public const string CARD_DELIVERIES_ENTRYTYPE = "Deliveries (Cards)";
        public const string GENERAL_ENTRYTYPE = "General";

        // Assignment Status

        public const string ASSIGNED_ASSIGNMENT_STATUS = "Assigned";
        public const string IN_PROGRESS_ASSIGNMENT_STATUS = "In Progress";
        public const string SHIPPING_ASSIGNMENT_STATUS = "Shipping";
        public const string COMPLETED_ASSIGNMENT_STATUS = "Completed";


    }
}
