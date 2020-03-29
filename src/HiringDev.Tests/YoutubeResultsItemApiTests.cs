using System;
using System.Threading.Tasks;
using HiringDev.Service.Controllers;
using HiringDev.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HiringDev.Tests
{
    public class YoutubeResultsItemApiTests
    {
        private ItemController _controller;
        private readonly IYoutubeResultsRepository _repository;

        public YoutubeResultsItemApiTests()
        {
            _repository = new YoutubeResultsRepositoryMock();
            _controller = new ItemController(_repository);
        }

        [Fact]
        public async Task ShouldReturnOKWhenTermExists()
        {
            var results = await _controller.Get("1", "ABC1234");
            Assert.IsType<OkObjectResult>(results);
        }

        [Fact]
        public async Task ShouldReturnNorNullOrEmptyWhenExists()
        {
            var results = await _controller.Get("0", "ABC4321") as OkObjectResult;
            var items = Assert.IsType<YoutubeResult>(results.Value);

            Assert.NotNull(items);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenTermDoesntExists()
        {
            var results = await _controller.Get("1", "fgfegfdheh");
            Assert.IsType<NotFoundResult>(results);
        }

    }
}