namespace Profile.Logic.Models.Auth0_Response_Models
{
    public class Auth0IdentityModel
    {
        public string user_id { get; set; }
        public string provider { get; set; }
        public string connection { get; set; }
        public bool isSocial { get; set; }
    }
}
