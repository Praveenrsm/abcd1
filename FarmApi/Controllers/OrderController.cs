using FarmBusiness.Services;
using FarmTradeEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private OrderService _orderService;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPut("CancelOrder")]
        public IActionResult CancelOrder(int orderId)
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            Guid userId = Guid.Parse(claim.Value);

            _orderService.CancelOrder(orderId, userId);
            return Ok(new { message = "Order cancelled successfully" });
        }

        [HttpPost("PlaceOrder")]
        public IActionResult PlaceOrder( int adddress)
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (claim == null)
                return Unauthorized("Invalid or expired token");

            Guid userId = Guid.Parse(claim.Value);

            int orderId=_orderService.PlaceOrder(userId,adddress);
            return Ok(new
            {
                OrderId = orderId,
                message = "Order placed successfully"
            });
        }

        [HttpGet("GetOrderDetails")]
        public IActionResult GetOrderDetails(int orderId)
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (claim == null)
                return Unauthorized("Invalid or expired token");

            Guid userId = Guid.Parse(claim.Value); // ✅ NON-nullable
            var result = _orderService.GetOrderDetails(orderId,userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}
