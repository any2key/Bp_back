namespace Bp_back.Models.Identity
{
    public class TokenResponseData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
        public string Login { get; set; }
        public string UserRole { get; set; }
    }

    public class ValidateRefreshTokenResponseData
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        public string Login { get; set; }

    }
}
