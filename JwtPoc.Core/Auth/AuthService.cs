using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtPoc.Core.Auth.Dto;
using JwtPoc.Data.User;
using JwtPoc.Data.User.Models;
using Microsoft.IdentityModel.Tokens;

namespace JwtPoc.Core.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;

        public AuthService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<(byte[] passwordHash, byte[] passwordSalt)> HashPasswordAsync(string password)
        {
            using var hmac = new HMACSHA512();
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var salt = hmac.Key;
            return await Task.FromResult((hash, salt));
        }

        public async Task<Jwt> LoginAsync(UserLoginDto loginData)
        {
            var user = await _repo.GetUserAsync(u => u.Username == loginData.Username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPassword(user, loginData.Password))
            {
                return null;
            }

            var claims = BuildClaims(user);
            var rawJwt = GetJwt(claims);

            var jwt = new JwtSecurityTokenHandler().WriteToken(rawJwt);

            return new Jwt
            {
                SecurityToken = jwt,
                RefreshToken = "not implemented",
                ExpiryDate = GetTokenExpiryDate
            };
        }

        private IEnumerable<Claim> BuildClaims(UserModel user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
        }

        private SecurityToken GetJwt(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-mega-secret-key"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var expiresAt = GetTokenExpiryDate;

            return new JwtSecurityToken("JwtPoc", "JwtAud", claims, expires: expiresAt,
                signingCredentials: credentials);
        }

        private bool VerifyPassword(UserModel user, string password)
        {
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (var i = 0; i < password.Length; i++)
            {
                if (user.PasswordHash[i] != computedHash[i])
                {
                    return false;
                }
            }

            return true;
        }

        private DateTime GetTokenExpiryDate => DateTime.UtcNow.AddMinutes(60);
    }
}
