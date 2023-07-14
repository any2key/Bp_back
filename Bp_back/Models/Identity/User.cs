using System.ComponentModel.DataAnnotations;

namespace Bp_back.Models.Identity
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

    }
}
