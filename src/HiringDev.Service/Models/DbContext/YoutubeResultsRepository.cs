using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<YoutubeResult> GetResult(string id)
        {
            return _context
                    .YoutubeResultSet
                    .Find(Builders<YoutubeResult>.Filter.Eq("id", ObjectId.Parse(id)))
                    .FirstOrDefaultAsync();
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
            var updateResult =
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
            var deleteResult = await _context
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


    public interface IYoutubeResultsRepository
    {
        Task<IEnumerable<YoutubeResult>> GetAllResults();
        Task<YoutubeResult> GetResult(string id);
        Task<YoutubeResult> GetResult(YoutubeResultType type, string contentId);
        Task<List<YoutubeResult>> GetResultsByTerm(string term);
        Task Create(YoutubeResult item);
        Task<bool> Update(YoutubeResult item);
        Task<bool> Delete(YoutubeResultType type, string contentId);
    }
}