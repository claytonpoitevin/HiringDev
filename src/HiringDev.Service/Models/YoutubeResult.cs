using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HiringDev.Service.Models
{
    public class YoutubeResult
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string ContentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }
        public YoutubeResultType Type { get; set; }
    }

    public enum YoutubeResultType {
        Channel,
        Video
    }
}