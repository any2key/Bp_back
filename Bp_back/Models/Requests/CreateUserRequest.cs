namespace Bp_back.Models.Requests
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
    }
}
