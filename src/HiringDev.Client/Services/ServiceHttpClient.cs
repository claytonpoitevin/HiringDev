using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HiringDev.Client.Models;
using Newtonsoft.Json;

namespace HiringDev.Client.Services
{
    public class ServiceHttpClient : IServiceHttpClient
    {
        private readonly HttpClient _client;

        public ServiceHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<YoutubeDetailedResultModel> GetContent(string type, string id)
        {
            var strResponse = await _client.GetStringAsync($"/item/{type}/{id}");
            var item = JsonConvert.DeserializeObject<YoutubeDetailedResultModel>(strResponse);
            return item;
        }

        public async Task<List<YoutubeResultModel>> GetResultsByTerm(string term)
        {
            var strResponse = await _client.GetStringAsync($"/search/{term}");
            var items = JsonConvert.DeserializeObject<List<YoutubeResultModel>>(strResponse);
            return items;
        }
    }

    public interface IServiceHttpClient
    {
        Task<List<YoutubeResultModel>> GetResultsByTerm(string term);
        Task<YoutubeDetailedResultModel> GetContent(string type, string id);
    }
}