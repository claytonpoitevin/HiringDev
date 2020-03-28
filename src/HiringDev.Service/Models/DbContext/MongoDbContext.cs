using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HiringDev.Service.Models
{
    public interface IMongoDbContext
    {
        IMongoCollection<YoutubeResult> YoutubeResultSet { get; }
    }
    
    public class MongoDbContext : IMongoDbContext
    {
        private IMongoDatabase _database { get; }

        public IMongoCollection<YoutubeResult> YoutubeResultSet
        {
            get
            {
                return _database.GetCollection<YoutubeResult>("YoutubeResult");
            }
        }

        public MongoDbContext(IOptions<Settings> options, IMongoClient client)
        {
            try
            {
                _database = client.GetDatabase(options.Value.Database);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível se conectar com o servidor.", ex);
            }
        }

    }
}