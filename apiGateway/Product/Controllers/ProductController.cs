using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Products(int id)
        {
            return Ok(new { id=id,name="praveen",age=25 });
        }
    }
}
