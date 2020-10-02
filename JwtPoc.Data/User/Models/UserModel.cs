using System;

namespace JwtPoc.Data.User.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public UserModel()
        {
            UserId = Guid.NewGuid();
        }
    }
}
