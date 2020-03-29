using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using HiringDev.Service.Models;
using Microsoft.Extensions.Options;

namespace HiringDev.Service.External
{
    public class YoutubeServiceProvider : IYoutubeServiceProvider
    {
        private string _apikey = "";
        private string _appname = "";

        public YoutubeServiceProvider(IOptions<Settings> options)
        {
            _apikey = options.Value.YouTubeApiKey;
            _appname = options.Value.YouTubeAppname;
        }

        public async Task<IList<SearchResult>> SearchAsync(string term)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _apikey,
                ApplicationName = _appname
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = term;
            searchListRequest.MaxResults = 50;

            var searchListResponse = await searchListRequest.ExecuteAsync();
            return searchListResponse.Items;
        }
    }

    public class YoutubeServiceProviderMock : IYoutubeServiceProvider
    {
        private List<SearchResult> _mockSearchResults = new List<SearchResult>();

        public YoutubeServiceProviderMock()
        {
            _mockSearchResults.Add(new SearchResult()
            {
                Kind = "youtube#searchResult",
                Id = new ResourceId() { VideoId = "123", Kind = "youtube#video" },
                Snippet = new SearchResultSnippet()
                {
                    Title = $"Teste 'abcde'",
                    Description = $"Descrição 1234 ",
                    Thumbnails = new ThumbnailDetails()
                    {
                        High = new Thumbnail() {Url = "https://user-images.githubusercontent.com/16944/32996962-97829150-cd3e-11e7-99a7-656c6135d162.png" }
                    }
                }
            });
        }

        public Task<IList<SearchResult>> SearchAsync(string term)
        {
            var lista = _mockSearchResults.Where(x => x.Snippet.Title.ToLower().Contains(term.ToLower()) || x.Snippet.Description.ToLower().Contains(term.ToLower())).ToList();

            return Task.FromResult(lista as IList<SearchResult>);
        }
    }

    public interface IYoutubeServiceProvider
    {
        Task<IList<SearchResult>> SearchAsync(string term);
    }
}