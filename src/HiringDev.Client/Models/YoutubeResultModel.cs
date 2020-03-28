using System;

namespace HiringDev.Client.Models
{
    public class YoutubeResultModel
    {
        public string ContentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }
        public string Type { get; set; }
    }

    public class YoutubeDetailedResultModel : YoutubeResultModel
    {
        public string Link
        {
            get
            {
                if (Type == "1") return $"https://www.youtube.com/watch?v={ContentId}";
                return $"https://www.youtube.com/channel/{ContentId}";
            }
        }
        public string ChannelTitle { get; set; }
        public DateTime? PublishedAt { get; set; }

    }
}