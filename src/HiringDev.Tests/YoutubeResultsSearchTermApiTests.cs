using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiringDev.Service.Controllers;
using HiringDev.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HiringDev.Tests
{
    public class YoutubeResultsSearchTermApiTests
    {
        private SearchController _controller;
        private readonly IYoutubeResultsRepository _repository;

        public YoutubeResultsSearchTermApiTests()
        {
            _repository = new YoutubeResultsRepositoryMock();
            _controller = new SearchController(_repository);
        }

        [Fact]
        public async Task ShouldReturnOKWhenTermExists()
        {
            var results = await _controller.Get("test");
            Assert.IsType<OkObjectResult>(results);
        }


        [Fact]
        public async Task ShouldReturn3ItemsWhenTermExists()
        {
            var results = await _controller.Get("test") as OkObjectResult;
            var items = Assert.IsType<List<YoutubeResult>>(results.Value);
            Assert.Equal(3, items.Count);
        }


        [Fact]
        public async Task ShouldReturnNotFoundWhenTermDoesntExists()
        {
            var results = await _controller.Get("abcdefgh");
            Assert.IsType<NotFoundResult>(results);
        }

        // get por tipo e id
        // quando procurar por um item especifico qualquer deve retornar notfound
        // quando procurar por um item especifico exitente deve retornar o ok result
        //quando procurar por um item especifico exitente deve retornar o item

    }

    public class YoutubeResultsRepositoryMock : IYoutubeResultsRepository
    {
        private List<YoutubeResult> _dbCollectionMock = new List<YoutubeResult>();

        public YoutubeResultsRepositoryMock()
        {
            _dbCollectionMock.Add(new YoutubeResult()
            {
                Id = new MongoDB.Bson.ObjectId(DateTime.Now, 1, 1, 1),
                ContentId = "ABC1234",
                Title = "Video 1",
                Description = "Video de teste",
                Type = YoutubeResultType.Video
            });
            _dbCollectionMock.Add(new YoutubeResult()
            {
                Id = new MongoDB.Bson.ObjectId(DateTime.Now, 1, 1, 2),
                ContentId = "ABC4321",
                Title = "Canal 1",
                Description = "Bem vindos ao meu canal",
                Type = YoutubeResultType.Channel
            });
            _dbCollectionMock.Add(new YoutubeResult()
            {
                Id = new MongoDB.Bson.ObjectId(DateTime.Now, 1, 1, 3),
                ContentId = "XYZ0000",
                Title = "Video 2",
                Description = "Video de teste de novo",
                Type = YoutubeResultType.Video
            });
            _dbCollectionMock.Add(new YoutubeResult()
            {
                Id = new MongoDB.Bson.ObjectId(DateTime.Now, 1, 1, 4),
                ContentId = "XYZ0000",
                Title = "Video 3",
                Description = "Video de teste de novo 2",
                Type = YoutubeResultType.Video
            });
        }

        public Task Create(YoutubeResult item)
        {
            _dbCollectionMock.Add(item);
            return Task.Delay(20);
        }

        public Task<bool> Delete(YoutubeResultType type, string contentId)
        {
            var item = GetResult(type, contentId);
            _dbCollectionMock.Remove(item.Result);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<YoutubeResult>> GetAllResults()
        {
            return Task.FromResult(_dbCollectionMock.AsEnumerable());
        }

        public Task<YoutubeResult> GetResult(YoutubeResultType type, string contentId)
        {
            var item = _dbCollectionMock.FirstOrDefault(x => x.Type == type && x.ContentId == contentId);
            return Task.FromResult(item);
        }

        public Task<YoutubeResult> GetResult(string id)
        {
            var oid = MongoDB.Bson.ObjectId.Parse(id);
            var item = _dbCollectionMock.FirstOrDefault(x => x.Id == oid);
            return Task.FromResult(item);
        }

        public Task<List<YoutubeResult>> GetResultsByTerm(string term)
        {
            var items = _dbCollectionMock.Where(x => x.Title.Contains(term) || x.Description.Contains(term)).ToList();
            return Task.FromResult(items);
        }

        public async Task<bool> Update(YoutubeResult item)
        {
            var found = await GetResult(item.Type, item.ContentId);
            found.Title = item.Title;
            found.Description = item.Description;
            found.ImageUri = item.ImageUri;

            return true;
        }
    }
}
