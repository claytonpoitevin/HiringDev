using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;
using HiringDev.Service.External;

namespace HiringDev.Service.Models
{
    public class YoutubeLookupService : IYoutubeLookupService
    {
        IYoutubeServiceProvider _serviceProvider;
        IYoutubeResultsRepository _repository;

        public YoutubeLookupService(IYoutubeServiceProvider serviceProvider, IYoutubeResultsRepository repository)
        {
            _serviceProvider = serviceProvider;
            _repository = repository;
        }

        public async Task<List<YoutubeResult>> SearchAsync(string term)
        {
            // encontrar resultados no yt e inserir/atualizar na base
            var ytResults = await _serviceProvider.SearchAsync(term);
            var items = PrepareResults(ytResults);
            await InsertOrUpdateResults(items);

            return items;
        }

        private List<YoutubeResult> PrepareResults(IList<SearchResult> ytResults)
        {
            var validtypes = new[] { "youtube#channel", "youtube#video" };
            var items = ytResults.Where(x => validtypes.Contains(x.Id.Kind)).Select(
                x => new YoutubeResult()
                {
                    ContentId = x.Id.Kind == "youtube#channel" ? x.Id.ChannelId : x.Id.VideoId,
                    Title = x.Snippet.Title,
                    Description = x.Snippet.Description,
                    ImageUri = x.Snippet.Thumbnails.Maxres?.Url ?? x.Snippet.Thumbnails.High?.Url ?? x.Snippet.Thumbnails.Default__?.Url,
                    Type = x.Id.Kind == "youtube#channel" ? YoutubeResultType.Channel : YoutubeResultType.Video,
                }).ToList();
            return items;
        }

        private async Task InsertOrUpdateResults(List<YoutubeResult> items)
        {
            foreach (var item in items)
            {
                await InsertOrUpdateResult(item);
            }
        }

        private async Task InsertOrUpdateResult(YoutubeResult item)
        {
            var itemdb = _repository.GetResult(item.Type, item.ContentId);
            if (itemdb == null) await _repository.Create(item);
        }

    }



    public interface IYoutubeLookupService
    {
        Task<List<YoutubeResult>> SearchAsync(string term);
    }
}