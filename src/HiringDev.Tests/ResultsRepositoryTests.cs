using System;
using System.Threading.Tasks;
using HiringDev.Service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Xunit;

namespace HiringDev.Tests
{
    public class ResultsRepositoryTests
    {
        private readonly IYoutubeResultsRepository _serviceProvider;

        public ResultsRepositoryTests()
        {
            _serviceProvider = new YoutubeResultsRepositoryMock();
            // var client = new MongoClient("mongodb://localhost:27017");
            // var someOptions = Options.Create(new Settings() { ConnectionString = "mongodb://localhost:27017", Database = "SegfyDb" });

            // var mongodb = new MongoDbContext(someOptions, client);
            // _serviceProvider = new YoutubeResultsRepository(mongodb);
        }

        [Fact]
        public async Task ShouldCreateNewItemCorrectly()
        {
            var id = System.IO.Path.GetTempFileName();
            var item = new YoutubeResult()
            {
                Title = "Teste 1",
                Description = "Descrição 1",
                Type = YoutubeResultType.Video,
                ContentId = id,
                PublishedAt = DateTime.Now,

            };
            await _serviceProvider.Create(item);

            var createdItem = await _serviceProvider.GetResult(YoutubeResultType.Video, id);
            Assert.NotNull(createdItem);
        }

        [Fact]
        public async Task ShouldReadExistingItems()
        {
            var items = await _serviceProvider.GetAllResults();
            Assert.NotNull(items);
        }

        [Fact]
        public async Task ShouldDeleteExistingItem()
        {
            var id = System.IO.Path.GetTempFileName();
            var item = new YoutubeResult()
            {
                Title = "Teste 1",
                Description = "Descrição 1",
                Type = YoutubeResultType.Video,
                ContentId = id,
                PublishedAt = DateTime.Now,

            };
            await _serviceProvider.Create(item);

            var deleted = await _serviceProvider.Delete(YoutubeResultType.Video, id);
            Assert.True(deleted);

            var existingItem = await _serviceProvider.GetResult(YoutubeResultType.Video, id);
            Assert.Null(existingItem);
        }

        [Fact]
        public async Task ShouldUpdateExistingItem()
        {
            var id = System.IO.Path.GetTempFileName();
            var item = new YoutubeResult()
            {
                Title = "Teste 1",
                Description = "Descrição 1",
                Type = YoutubeResultType.Video,
                ContentId = id,
                PublishedAt = DateTime.Now,

            };
            await _serviceProvider.Create(item);

            item.Title = "UPDATED";
            await _serviceProvider.Update(item);

            var existingItem = await _serviceProvider.GetResult(YoutubeResultType.Video, id);
            Assert.Equal(existingItem.Title, "UPDATED");
        }
    }
}