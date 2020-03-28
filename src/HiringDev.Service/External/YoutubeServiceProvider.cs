using System;
using System.Collections.Generic;
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
        public Task<IList<SearchResult>> SearchAsync(string term)
        {

            var lista = new List<SearchResult>();
            lista.Add(new SearchResult()
            {
                Kind = "youtube#video",
                Id = new ResourceId() { VideoId = "123" },
                Snippet = new SearchResultSnippet() { Title = $"Teste '{term}'", Description = $"Descrição '{term}' " }
            });
            return Task.FromResult(lista as IList<SearchResult>);
        }
    }

    public interface IYoutubeServiceProvider
    {
        Task<IList<SearchResult>> SearchAsync(string term);
    }
}