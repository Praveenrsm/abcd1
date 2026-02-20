using FarmBusiness.Services;
using FarmTradeEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private CartService _cartService;
        private UserCartService _userCartService;
        public CartController(CartService cartService, UserCartService userCartService)
        {
            _cartService = cartService;
            _userCartService = userCartService;
        }
        [HttpPost("AddCart")]
        public IActionResult AddToCart([FromBody] Cart cart)
        {
            Guid? userId = null;

            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim != null)
                userId = Guid.Parse(claim.Value);
            _cartService.AddToCart(cart,userId);
            return Ok(cart);
        }
        //[HttpPost("AddUserCart")]
        //public IActionResult AddToCart(Guid userId, [FromBody] Cart cart)
        //{
        //    _userCartService.AddToCart(userId, cart);
        //    return Ok(cart);
        //}
        [HttpPut("UpdateQuantity")]
        public IActionResult EditToCart(int productId,int quantity)
        {
            Guid? userId = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid or expired token");

            userId = Guid.Parse(userIdClaim.Value);
            _cartService.UpdateQuantity(productId, userId,quantity);
            return Ok("cart Details updated successfully");
        }
        //[HttpPut("UpdateUserQuantity")]
        //public IActionResult EditToCart(Guid userId,int productId, int quantity)
        //{
        //    _userCartService.UpdateQuantity(userId,productId, quantity);
        //    return Ok("cart Details for a user updated successfully");
        //}
        [HttpGet("GetCart")]
        public IEnumerable<Cart> GetCarts()
        {
            Guid? userId = null;

            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim != null)
                userId = Guid.Parse(claim.Value);
            return _cartService.GetCartDetails(userId);
        }
        //[HttpGet("GetUserCart")]
        //public IEnumerable<Cart> GetCarts(Guid userId)
        //{
        //    return _userCartService.GetCartItems(userId);
        //}
    }
}
