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
                return BadRequest("Invalid product data");

            _DynamicProductConetxt.products.Add(Dynamic);
            _DynamicProductConetxt.SaveChanges();

            // AFTER SaveChanges → ProductId is generated
            return Ok(new
            {
                message = "Product added successfully",
                productId = Dynamic.ProductId
            });
        }

        [HttpGet("GetDynamicProduct")]
        public IEnumerable<Product1> GetDynamicProducts()
        {
            return _DynamicProductConetxt.products.ToList();
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
            var productDelete = _DynamicProductConetxt.products.Find(productId);
            _DynamicProductConetxt.products.Remove(productDelete);
            _DynamicProductConetxt.SaveChanges();
            return Ok(productDelete.ProductId + "Deleted Successfully");
            #endregion
        }
        //[HttpGet("GetDynamicProductById")]
        //public IActionResult GetDynamicProductById(int id)
        //{
        //    var result = _DynamicProductConetxt.products.ToList();
        //    var GetAProduct = result.Where(obj => obj.ProductId == id).FirstOrDefault();
        //    return Ok(GetAProduct);
        //}

        [HttpGet("GetDynamicProductById/{id}")]
        public IActionResult GetDynamicProductById(int id)
        {
            var product = _DynamicProductConetxt.products
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

    }
}
