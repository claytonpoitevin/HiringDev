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
        public async Task WhenGetShouldReturnOkWhenFindingItem()
        {
            var id = new MongoDB.Bson.ObjectId(DateTime.Now, 1, 1, 2);
            var results = await _controller.Get(id.ToString());
            Assert.IsType<OkObjectResult>(results);
        }

        [Fact]
        public async Task WhenGetShouldReturnCorrectItem()
        {
            var id = new MongoDB.Bson.ObjectId(DateTime.Now, 1, 1, 2);
            var results = await _controller.Get(id.ToString()) as OkObjectResult;
            var item = Assert.IsType<YoutubeResult>(results.Value);

            Assert.Equal(item.Id, id);
        }

        [Fact]
        public async Task WhenGetInvalidShouldReturnNotFound()
        {
            var id = new MongoDB.Bson.ObjectId(DateTime.Now, 1, 1, 99);
            var results = await _controller.Get(id.ToString());
            Assert.IsType<NotFoundResult>(results);
        }
        
        // ao criar item precisa retornar ok
        // ao crair item precisa realmente adicionar o item 
        // ao criar item ja existente precisa substituir o anterior pela chave



        // [Fact]
        // public async Task ShouldReturnOkWhenFindingItem() {
        //     var results = await _controller.Get("1");
        //     Assert.IsType<OkObjectResult>(results);
        // }

        // [Fact]
        // public async Task ShouldHaveCreatedTheItem() {
        //     var results = await _controller.Get(new MongoDB.Bson.ObjectId(DateTime.Now, 1, 1, 2).ToString());
        //     Assert.IsType<OkObjectResult>(results);
        // }

        // [Fact]
        // public async Task ShouldHaveCreatedTheItem() {
        //     var results = await _controller.Get(new MongoDB.Bson.ObjectId(DateTime.Now, 1, 1, 2).ToString());
        //     Assert.IsType<OkObjectResult>(results);
        // }
    }
}