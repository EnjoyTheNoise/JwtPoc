using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JwtPoc.Data.User.Models;

namespace JwtPoc.Data.User
{
    public class UserRepository : IUserRepository
    {
        private readonly List<UserModel> _users;

        public UserRepository()
        {
            _users = new List<UserModel>();
        }

        public async Task<bool> AddUserAsync(UserModel newUser)
        {
            try
            {
                if (_users.Any(u => u.Username == newUser.Username))
                {
                    return await Task.FromResult(false);
                }

                _users.Add(newUser);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> UserExistsAsync(Expression<Func<UserModel, bool>> predicate)
        {
            return await Task.FromResult(_users.Any(predicate.Compile()));
        }

        public async Task<UserModel> GetUserAsync(Expression<Func<UserModel, bool>> predicate)
        {
            try
            {
                if (!await UserExistsAsync(predicate))
                {
                    return await Task.FromResult((UserModel) null);
                }

                var user = _users.Single(predicate.Compile());

                return await Task.FromResult(user);
            }
            catch (Exception)
            {
                return await Task.FromResult((UserModel)null);
            }
        }
    }
}
