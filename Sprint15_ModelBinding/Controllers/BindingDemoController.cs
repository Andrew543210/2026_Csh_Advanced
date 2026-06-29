using Microsoft.AspNetCore.Mvc;
using Sprint15_ModelBinding.Models;

namespace Sprint15_ModelBinding.Controllers
{
    [ApiController]
    [Route("sprint15/[controller]")]
    public class BindingDemoController : ControllerBase
    {
       
        [HttpGet("shop/{category}")]
        public IActionResult GetProducts(
            [FromRoute] string category, 
            [FromQuery] string manager, 
            [FromQuery] int limit)
        {
            return Ok(new { Category = category, Manager = manager, Limit = limit });
        }

       
        [HttpGet("device")]
        public IActionResult GetDeviceDetails([FromHeader(Name = "X-Device-Os")] string osVersion)
        {
            return Ok(new { DeviceOperatingSystem = osVersion ?? "Unknown" });
        }

     
        [HttpPost("register-form")]
        public IActionResult RegisterViaForm([FromForm] [Bind("Username,Email,Age")] UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Ok(model);
        }

        [HttpPost("register-json")]
        public IActionResult RegisterViaJson([FromBody] UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { Message = "JSON parsed successfully!", Data = model });
        }
    }
}