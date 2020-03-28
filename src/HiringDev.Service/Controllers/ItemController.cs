using System.Threading.Tasks;
using HiringDev.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace HiringDev.Service.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IYoutubeResultsRepository _repository;
        public ItemController(IYoutubeResultsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var item = await _repository.GetResult(id);
            if (item == null) return new NotFoundResult();
            return new OkObjectResult(item);
        }

    }
}