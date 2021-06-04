using System;
using System.Collections.Generic;
using System.Text;

namespace Profile.Logic.Constants
{
    public static class SharkTankConstants
    {
        // Post Constants
        public const string CREATED_ASSIGNMENT_CATEGORY = "Assignments Given";
        public const string CREATED_NEW_MESSAGE_CATEGORY = "Created New Message";
        public const string CREATED_NEW_CLIENT_CATEGORY = "New Client Created";
        public const string CREATED_NEW_AUTH0_CLIENT_CATEGORY = "New Auth0 Account Created";
        public const string CREATED_NEW_TAG_CATEGORY = "Added New Tag";
        public const string CREATED_NEW_CLIENT_TAG_RELATIONSHIPS_CATEGORY = "Added New Tags To Client";

        // Get Constants
        public const string GET_ALL_CLIENT_CATEGORY = "Get All Clients";
        public const string GET_SPECIFIC_CLIENT_CATEGORY = "Get Client By ID";
        public const string GET_PROFILE_CATEGORY = "Get Profile";
        public const string GET_SPECIFIC_HISTORY_CATEGORY = "Get History";
        public const string GET_ALL_HISTORY_CATEGORY = "Get All History";

        // Put Constants
        public const string MODIFIED_CLIENT_CATEGORY = "Modified Client";
        public const string MODIFIED_PROFILE_CATEGORY = "Modified Profile";
        public const string MODIFIED_ANSWER_CATEGORY = "Modified Answer";
        public const string MODIFIED_ASSIGNMENT_STATUS_CATEGORY = "Modified Assignment Status";
        public const string MODIFIED_CLIENT_STATUS_CATEGORY = "Modified Client Status";
        public const string MODIFIED_MESSAGE_READ_STATUS_CATEGORY = "Modified Message Read Status";

        // Delete Constants
        public const string DELETED_CLIENT_CATEGORY = "Deleted Client";
        public const string DELETED_ASSIGNMENT_CATEGORY = "Deleted Assignment";
        public const string DELETED_TAG_CATEGORY = "Deleted Tag";
    }
}
