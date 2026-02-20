using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Hubs;

namespace SignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hub;
        public SignalRController(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromQuery] string user, [FromQuery] string message)
        {
            await _hub.Clients.All.SendAsync("ReceiveMessage", user, message);
            return Ok(new { Status = "Message Sent" });
        }
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("SignalR Controller is working");
        }
    }
}
