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
        private readonly IYoutubeResultsRepository _repository;
        public SearchController(IYoutubeResultsRepository repository)
        {
            _repository = repository;
        }



        [HttpGet("{term}", Name = "Get")]
        public async Task<IActionResult> Get(string term)
        {
            var results = await _repository.GetResultsByTerm(term);
            if (results == null || !results.Any())
                return new NotFoundResult();
            return new OkObjectResult(results);
        }

    }
}