using Azure.Core;
using FarmApi.Model;
using FarmBusiness.Services;
using FarmTradeEntity;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FarmApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserService _userService;
        private ICacheProvider _cacheProvider;
        private readonly IConfiguration _config;
        private readonly IAppCache _cache;

        public AccountController(UserService userService, ICacheProvider cacheProvider, IConfiguration config, IAppCache cache)
        {
            _userService = userService;
            _cacheProvider = cacheProvider;
            _config = config;
            _cache = cache;
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            #region Edit User:
            await _userService.UpdateUser(user);
            _cacheProvider.Remove(CacheKeys.User);
            return Ok("User Details updated successfully");
            #endregion
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            #region Register
            await _userService.AddUser(user);
            _cacheProvider.Remove(CacheKeys.User);
            return Ok("Register successfully!!");
            #endregion
        }

        [NonAction] 
        public string GenerateToken(User user)
        {
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            //}

            var claims = new[]
            {
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.Role, user.role),
               new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecretKeyWithAtLeast128Bits"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User request)
        {
            #region Login with User Email and password using Tokens
            var user = await _userService.Login(request.Email, request.password);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var token = GenerateToken(user);
            return Ok(new { message = "Login successful", token, user.Email, user.role });
            #endregion
        }

        [HttpGet("GetUserById")]
        public async Task<User> GetUserById(Guid userId)
        {
            #region Get User By Id
            //_cacheProvider.Set($"user_{userId}");
            return await _userService.GetUserById(userId);
            #endregion
        }
        [HttpGet("GetUsers")]
        public async Task<IEnumerable<User>> GetUsers(int pageNumber, int pageSize)
        {
            string cacheKey = $"Users-{pageNumber}-{pageSize}";

            var entry = await _cache.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var expiry = DateTimeOffset.Now.AddMinutes(2);
                    Console.WriteLine($"CACHE MISS → DB HIT. Cache expires at {expiry:HH:mm:ss}");

                    return new CacheEntry<IEnumerable<User>>
                    {
                        Data = await _userService.GetUsers(pageNumber, pageSize),
                        ExpiryTime = expiry
                    };

                },
                DateTimeOffset.Now.AddMinutes(2)
            );

            var remaining = entry.ExpiryTime - DateTimeOffset.Now;
            Console.WriteLine($"CACHE HIT → Remaining TTL: {remaining.TotalSeconds:F0} seconds");

            return entry.Data;
        }

        //[HttpGet("GetUsers")]
        //public async Task<IEnumerable<User>> GetUsers(int pageNumber, int pageSize)
        //{
        //    string cacheKey = $"Users-{pageNumber}-{pageSize}";

        //    return await _cache.GetOrAddAsync(cacheKey, async () =>
        //    {
        //        Console.WriteLine("Hitting database...");
        //        return await _userService.GetUsers(pageNumber, pageSize);
        //    },
        //    DateTimeOffset.Now.AddMinutes(2));    // cache duration
        //}
        //[HttpGet("GetUsers")]
        //public async Task<IEnumerable<User>> GetUsers(int pageNumber, int pageSize)
        //{
        //    if (!_cacheProvider.TryGetValue(CacheKeys.User, out IEnumerable<User> user))
        //    {
        //        user = await _userService.GetUsers(pageNumber, pageSize);

        //        var cacheMemoryEntryOption = new MemoryCacheEntryOptions
        //        {
        //            AbsoluteExpiration = DateTime.Now.AddMinutes(1),
        //            SlidingExpiration = TimeSpan.FromSeconds(30),
        //            Size = 1000
        //        };
        //        _cacheProvider.Set(CacheKeys.User, user, cacheMemoryEntryOption);
        //    }
        //    //#region Get User:
        //    return user;
        //    //#endregion
        //}
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            #region Delete User
            await _userService.DeleteUser(userId);
            _cacheProvider.Remove(CacheKeys.User);
            return Ok("User deleted successfully!!!");
            #endregion
        }
    }
}
