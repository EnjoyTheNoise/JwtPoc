using System;

namespace JwtPoc.Core.Auth.Dto
{
    public class Jwt
    {
        public string SecurityToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
