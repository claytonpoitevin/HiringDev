using Microsoft.AspNetCore.Mvc;

namespace HiringDev.Service.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Get()
        {
            return new OkResult();
        }

        [HttpGet("index")]
        public IActionResult GetIndex()
        {
            return new OkResult();
        }
    }
}