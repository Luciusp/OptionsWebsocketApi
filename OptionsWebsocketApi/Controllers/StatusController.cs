using Microsoft.AspNetCore.Mvc;

namespace OptionsWebsocketApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public IActionResult Status()
        {
            return Ok();
        }
    }
}
