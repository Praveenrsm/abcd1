using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace signalRMVC.Controllers
{
    public class SignalRController : Controller
    {
        
        public IActionResult SendMessage()
        {
            return View();
        }

        public IActionResult ChatApp()
        {
            return View();
        }

        // Optional: simple test endpoint
        [HttpGet("test-signalr")]
        public IActionResult Test()
        {
            return Ok("SignalR controller is working");
        }
    }
}
