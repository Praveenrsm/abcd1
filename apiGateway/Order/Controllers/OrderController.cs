using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly HttpClient HttpClient;
        public OrderController(IHttpClientFactory httpClient)
        {
            HttpClient = httpClient.CreateClient();
        }
        [HttpGet("productId")]
        public async Task<IActionResult> Orders(int productId)
        {
            var response = await HttpClient.GetStringAsync($"https://localhost:7233/api/Product/{productId}");
            return Ok(new { orderId = 1, productId = response, Status = "order placed" });
        }
    }
}
