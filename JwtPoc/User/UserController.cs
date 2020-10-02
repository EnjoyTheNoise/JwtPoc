using System.Threading.Tasks;
using JwtPoc.Core.User;
using JwtPoc.Core.User.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtPoc.Api.User
{
    [Authorize]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] AddUserDto request)
        {
            var isSuccess = await _userService.AddUserAsync(request);

            return isSuccess ? (IActionResult) NoContent() : BadRequest();
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUsernameCuzWhyNot(string username)
        {
            var result = await _userService.GetUserAsync(username);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
