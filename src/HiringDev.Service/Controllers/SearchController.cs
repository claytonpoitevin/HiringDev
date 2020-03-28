using System.Linq;
using System.Threading.Tasks;
using HiringDev.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace HiringDev.Service.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IYoutubeLookupService _lookup;
        public SearchController(IYoutubeLookupService lookup)
        {
            _lookup = lookup;
        }

        [HttpGet("{term}")]
        public async Task<IActionResult> Get(string term)
        {
            var results = await _lookup.SearchAsync(term);
            if (results == null || !results.Any())
                return new NotFoundResult();
            return new OkObjectResult(results);
        }

    }
}