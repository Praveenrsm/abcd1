using FarmBusiness.Services;
using Microsoft.AspNetCore.Http;
using FarmTradeEntity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using LazyCache;
using FarmApi.Model;
using Microsoft.Extensions.Caching.Memory;

namespace FarmApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserService _userService;
        private ICacheProvider cacheProvider;
        public AccountController(UserService userService,ICacheProvider _cacheProvider)
        {
            _userService = userService;
            _cacheProvider= cacheProvider;
        }
        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromBody] User user)
        {
            #region Edit User:
            _userService.UpdateUser(user);
            return Ok("User Details updated successfully");
            #endregion
        }

        [AllowAnonymous]
        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody] User user)
        {
            #region Register
            _userService.AddUser(user);
            return Ok("Register successfully!!");
            #endregion
        }

        [NonAction] 
        public string GenerateToken(string email, string role)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            var claims = new[]
            {
              new Claim(ClaimTypes.Email, email),
              new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecretKeyWithAtLeast128Bits"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               // issuer: "http://192.168.66.236:8080",
                //audience: "http://192.168.66.236:8080",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] User user)
        {

            #region Login
            var userId = user.UserId;
            var result = _userService.Login(user);
            // Check for valid roles
            if (result == "admin" || result == "supplier" || result=="user")
            {
                var token = GenerateToken(user.Email, result);
                return Ok(new { message = "Login successful", code = 1, role = result, token = token,userid= userId }); // Return role
            }
            else if (result == "Invalid")
            {
                return BadRequest(new { message = "Login failed", code = -1 });
            }
            else
            {
                return StatusCode(500, new { message = "An error occurred.", code = 0 });
            }
            #endregion
        }
        
        [HttpGet("GetUserById")]
        public User GetUserById(Guid userId)
        {
            #region Get User By Id
            return _userService.GetUserById(userId);
            #endregion
        }
        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers(int pageNumber, int pageSize)
        {
            if (!cacheProvider.TryGetValue(CacheKeys.User, out IEnumerable<User> user))
            {
                user = _userService.GetUsers(pageNumber, pageSize);
            }
            var cacheMemoryEntryOption = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                SlidingExpiration = TimeSpan.FromSeconds(30),
                Size = 1000
            };
            cacheProvider.Set(CacheKeys.User, user, cacheMemoryEntryOption);
            //#region Get User:
            return _userService.GetUsers(pageNumber, pageSize);
            //#endregion
        }
        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser(Guid userId)
        {
            #region Delete User
            _userService.DeleteUser(userId);
            return Ok("User deleted successfully!!!");
            #endregion
        }
    }
}
