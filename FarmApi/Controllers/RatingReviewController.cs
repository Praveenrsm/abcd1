using FarmBusiness.Services;
using FarmTradeEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Diagnostics.Contracts;
using System.Security.Claims;

namespace FarmApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RatingReviewController : ControllerBase
    {
        private RRService _rRService;
        public RatingReviewController(RRService rrService)
        {
            _rRService = rrService;
        }
        
        [HttpDelete("DeleteRR")]
        public IActionResult DeleteRR(int rRId)
        {
            #region Delete RR
            _rRService.DeleteRR(rRId);
            return Ok("RR deleted successfully!!!");
            #endregion
        }
        [HttpPost("AddOrUpdateReview")]
        public IActionResult AddOrUpdateReview(int productId, string comments, int rating)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid or expired token");

            var userId = Guid.Parse(userIdClaim.Value);
            _rRService.AddOrUpdateReview(productId, userId, comments,rating);
            return Ok("RR updated successfully!!!");
        }
        [HttpGet("GetAllReview")]
        public async Task<IEnumerable<ReviewsAndRatings>> GetAllReview()
        {
            return await _rRService.GetAllReview();
        }
    }
}
