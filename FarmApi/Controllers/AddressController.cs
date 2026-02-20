using FarmBusiness.Services;
using FarmTradeEntity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private AddressService _addressService;
        public AddressController(AddressService addressService)=> _addressService = addressService;
        [HttpPost("AddAddress")]
        public async Task<IActionResult> AddAddress([FromBody]Address address)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid or expired token");

            var userId = Guid.Parse(userIdClaim.Value);
            await _addressService.AddAddress(address,userId);
            return Ok("Address Added successfully");
        }
        [HttpPut("UpdateAddress")]
        public IActionResult UpdateAddress([FromBody] Address address)
        {
            _addressService.UpdateAddress(address);
            return Ok("Address Updated Successfully");
        }
        [HttpGet("GetAddressById")]
        public Address GetAddressById(int addressId)
        {
            #region Get Address By Id
            return _addressService.GetAddressById(addressId);
            #endregion
        }
        [HttpGet("GetAddresss")]
        public ActionResult<IEnumerable<Address>> GetAddress()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            Guid userId = Guid.Parse(claim.Value);
            #region Get Address:
            return Ok(_addressService.GetAddresses(userId));
            #endregion
        }
        [HttpDelete("DeleteAddress")]
        public IActionResult DeleteAddress(int addressId)
        {
            #region Delete Address
            _addressService.DeleteAddress(addressId);
            return Ok("Address deleted successfully!!!");
            #endregion
        }
    }
}
