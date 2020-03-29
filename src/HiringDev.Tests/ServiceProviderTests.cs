using System.Threading.Tasks;
using HiringDev.Service.External;
using HiringDev.Service.Models;
using Microsoft.Extensions.Options;
using Xunit;

namespace HiringDev.Tests
{
    public class ServiceProviderTests
    {
        private readonly IYoutubeServiceProvider _serviceProvider;

        public ServiceProviderTests()
        {
            _serviceProvider = new YoutubeServiceProviderMock();
            // var someOptions = Options.Create(new Settings() { YouTubeAppname = "", YouTubeApiKey = "" });
            // _serviceProvider = new YoutubeServiceProvider(someOptions);
        }

        [Fact]
        public async Task ShouldReturnItemsWhenTermExists()
        {
            var items = await _serviceProvider.SearchAsync("abc");
            Assert.NotEmpty(items);
        }


        [Fact]
        public async Task ShouldReturnEmptyWhenTermDoesNotExists()
        {
            var items = await _serviceProvider.SearchAsync("DHC#Q(*H*&#QHCR&*Q#RGN*&CFX*&FNG)*&#GN FW*)#FG*");
            Assert.Empty(items);
        }

    }
}