using System.Threading.Tasks;
using JwtPoc.Core.Auth;
using JwtPoc.Core.User.Dto;
using JwtPoc.Data.User;
using JwtPoc.Data.User.Models;

namespace JwtPoc.Core.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IAuthService _authService;

        public UserService(IUserRepository repo, IAuthService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        public async Task<bool> AddUserAsync(AddUserDto newUser)
        {
            if (await _repo.UserExistsAsync(u => u.Username == newUser.Username))
            {
                return false;
            }

            var (passwordHash, passwordSalt) = await _authService.HashPasswordAsync(newUser.Password);
            var userToAdd = new UserModel
            {
                Username = newUser.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            var isSuccess = await _repo.AddUserAsync(userToAdd);

            return isSuccess;
        }

        public async Task<UserDto> GetUserAsync(string username)
        {
            var result = await _repo.GetUserAsync(u => u.Username == username);
            var user = new UserDto {Username = result.Username};

            return user;
        }
    }
}
