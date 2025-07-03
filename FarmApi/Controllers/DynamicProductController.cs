using FarmTradeDataLayer;
using FarmTradeEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmApi.Model;
namespace FarmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicProductController : ControllerBase
    {
        private readonly ContextFarmModel _DynamicProductConetxt;

        public DynamicProductController(ContextFarmModel DynamicProductConetxt)
        {
            _DynamicProductConetxt = DynamicProductConetxt;
        }
        [HttpPost("AddDynamicProduct")]
        public IActionResult AddDynamicProduct([FromBody] Product1 Dynamic)
        {
            if (Dynamic == null)
            {
                return BadRequest("Invalid product data.");
            }

            var product = _DynamicProductConetxt.product.FirstOrDefault(p => p.ProductId == Dynamic.ProductId);

            // Check if product already exists
            if (product != null)
            {
                return Conflict("Product with this ID already exists.");
            }

            _DynamicProductConetxt.product.Add(Dynamic);
            _DynamicProductConetxt.SaveChanges();

            return Ok("Success");
        }

        [HttpGet("GetDynamicProduct")]
        public IEnumerable<Product1> GetDynamicProducts()
        {
            return _DynamicProductConetxt.product.ToList();
        }
        [HttpPut("UpdateDynamicProduct")]
        public IActionResult UpdateDynamicProduct([FromBody] Product1 dynamic)
        {
            #region EDIT Dynamic Product
            _DynamicProductConetxt.Entry(dynamic).State = EntityState.Modified;
            _DynamicProductConetxt.SaveChanges();
            return Ok("Updated Successfully");
            #endregion
        }
        [HttpDelete("DeleteDynamicProduct")]
        public IActionResult DeleteDynamicProduct(int productId)
        {
            #region DELETE Dynamic Product
            var productDelete = _DynamicProductConetxt.product.Find(productId);
            _DynamicProductConetxt.product.Remove(productDelete);
            _DynamicProductConetxt.SaveChanges();
            return Ok(productDelete.ProductId + "Deleted Successfully");
            #endregion
        }
        [HttpGet("GetDynamicProductById")]
        public IActionResult GetDynamicProductById(int id)
        {
            var result = _DynamicProductConetxt.product.ToList();
            var GetAProduct = result.Where(obj => obj.ProductId == id).FirstOrDefault();
            return Ok(GetAProduct);
        }
    }
}
