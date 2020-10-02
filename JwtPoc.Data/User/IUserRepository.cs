using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JwtPoc.Data.User.Models;

namespace JwtPoc.Data.User
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(UserModel newUser);
        Task<bool> UserExistsAsync(Expression<Func<UserModel, bool>> predicate);
        Task<UserModel> GetUserAsync(Expression<Func<UserModel, bool>> predicate);
    }
}
