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
            var results = await _controller.Get("abc");
            Assert.IsType<OkObjectResult>(results);
        }

        [Fact]
        public async Task ShouldReturnNorNullOrEmptyWhenExists()
        {
            var results = await _controller.Get("abc") as OkObjectResult;
            var items = Assert.IsType<List<YoutubeResult>>(results.Value);

            Assert.NotNull(items);
            Assert.NotEmpty(items);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenTermDoesntExists()
        {
            var results = await _controller.Get("CEH@&Q*CYR&*)#NC&*GCNRM*R#Q");
            Assert.IsType<NotFoundResult>(results);
        }


    }
}
