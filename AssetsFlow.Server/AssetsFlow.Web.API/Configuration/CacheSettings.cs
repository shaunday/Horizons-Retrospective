namespace HsR.Web.API.Configuration
{
    public class CacheSettings
    {
        public int CacheDurationMinutes { get; set; } = 5;
        public int PreloadPageCount { get; set; } = 3;
    }
} 