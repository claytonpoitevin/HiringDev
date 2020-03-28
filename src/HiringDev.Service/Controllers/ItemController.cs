using System;
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

        [HttpGet("{type}/{id}")]
        public async Task<IActionResult> Get(string type, string id)
        {
            var t = (YoutubeResultType)int.Parse(type);
            var item = await _repository.GetResult(t, id);
            if (item == null) return new NotFoundResult();
            return new OkObjectResult(item);
        }


        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var item = await _repository.GetAllResults();
            if (item == null) return new NotFoundResult();
            return new OkObjectResult(item);
        }

    }
}