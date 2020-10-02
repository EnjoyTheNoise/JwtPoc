using System.Threading.Tasks;
using JwtPoc.Core.Auth.Dto;

namespace JwtPoc.Core.Auth
{
    public interface IAuthService
    {
        Task<(byte[] passwordHash, byte[] passwordSalt)> HashPasswordAsync(string password);
        Task<Jwt> LoginAsync(UserLoginDto loginData);
    }
}
