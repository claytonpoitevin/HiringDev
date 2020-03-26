using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HiringDev.Service.Models
{
    public class YoutubeResultsRepository : IYoutubeResultsRepository
    {
        private readonly IMongoDbContext _context;
        public YoutubeResultsRepository(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<YoutubeResult>> GetAllResults()
        {
            return await _context
                            .YoutubeResultSet
                            .Find(_ => true)
                            .ToListAsync();
        }

        public Task<YoutubeResult> GetResult(YoutubeResultType type, string contentId)
        {
            return _context
                    .YoutubeResultSet
                    .Find(MakeKeyFilters(type, contentId))
                    .FirstOrDefaultAsync();
        }

        public async Task Create(YoutubeResult item)
        {
            await _context.YoutubeResultSet.InsertOneAsync(item);
        }

        public async Task<bool> Update(YoutubeResult game)
        {
            ReplaceOneResult updateResult =
                await _context
                        .YoutubeResultSet
                        .ReplaceOneAsync(
                            filter: g => g.Id == game.Id,
                            replacement: game);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(YoutubeResultType type, string contentId)
        {
            DeleteResult deleteResult = await _context
                                                .YoutubeResultSet
                                                .DeleteOneAsync(MakeKeyFilters(type, contentId));
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        private FilterDefinition<YoutubeResult> MakeKeyFilters(YoutubeResultType type, string contentId)
        {
            var filter1 = Builders<YoutubeResult>.Filter.Eq(m => m.Type, type);
            var filter2 = Builders<YoutubeResult>.Filter.Eq(m => m.ContentId, contentId);
            return filter1 & filter2;
        }

        public Task<List<YoutubeResult>> GetResultsByTerm(string term)
        {
            var filter1 = Builders<YoutubeResult>.Filter.Regex("Title", new BsonRegularExpression($".*{term}.*"));
            var filter2 = Builders<YoutubeResult>.Filter.Regex("Description", new BsonRegularExpression($".*{term}.*"));

            var filter = Builders<YoutubeResult>.Filter.Or(filter1, filter2);
            return _context
                    .YoutubeResultSet
                    .Find(filter)
                    .ToListAsync();
        }
    }

    public interface IYoutubeResultsRepository
    {
        Task<IEnumerable<YoutubeResult>> GetAllResults();
        Task<YoutubeResult> GetResult(YoutubeResultType type, string contentId);
        Task<List<YoutubeResult>> GetResultsByTerm(string term);
        Task Create(YoutubeResult item);
        Task<bool> Update(YoutubeResult item);
        Task<bool> Delete(YoutubeResultType type, string contentId);
    }
}