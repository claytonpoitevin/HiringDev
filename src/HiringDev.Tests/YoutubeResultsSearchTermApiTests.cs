using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiringDev.Service.Controllers;
using HiringDev.Service.External;
using HiringDev.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HiringDev.Tests
{
    public class YoutubeResultsSearchTermApiTests
    {
        private readonly SearchController _controller;
        private readonly IYoutubeLookupService _lookup;
        private readonly IYoutubeServiceProvider _serviceProvider;
        private readonly IYoutubeResultsRepository _repository;

        public YoutubeResultsSearchTermApiTests()
        {
            _serviceProvider = new YoutubeServiceProviderMock();
            _repository = new YoutubeResultsRepositoryMock();

            _lookup = new YoutubeLookupService(_serviceProvider, _repository);
            _controller = new SearchController(_lookup);
        }

        [Fact]
        public async Task ShouldReturnOKWhenTermExists()
        {
            var results = await _controller.Get("test");
            Assert.IsType<OkObjectResult>(results);
        }


        // [Fact]
        // public async Task ShouldReturn3ItemsWhenTermExists()
        // {
        //     var results = await _controller.Get("test") as OkObjectResult;
        //     var items = Assert.IsType<List<YoutubeResult>>(results.Value);
        //     Assert.Equal(3, items.Count);
        // }


        // [Fact]
        // public async Task ShouldReturnNotFoundWhenTermDoesntExists()
        // {
        //     var results = await _controller.Get("abcdefgh");
        //     Assert.IsType<NotFoundResult>(results);
        // }

        // [Fact]
        // public async Task ShouldReturnItemsWhenMinecraftTermIsGiven() // youtube = minecraft
        // {
        //     var results = await _controller.Get("minecraft") as OkObjectResult;
        //     var items = Assert.IsType<List<YoutubeResult>>(results.Value);
        //     Assert.NotEmpty(items);
        // }
        // get por tipo e id
        // quando procurar por um item especifico qualquer deve retornar notfound
        // quando procurar por um item especifico exitente deve retornar o ok result
        //quando procurar por um item especifico exitente deve retornar o item

    }
}
