using System;
using System.Threading.Tasks;
using JwtPoc.Core.User.Dto;

namespace JwtPoc.Core.User
{
    public interface IUserService
    {
        Task<bool> AddUserAsync(AddUserDto newUser);
        Task<UserDto> GetUserAsync(string userId);
    }
}
