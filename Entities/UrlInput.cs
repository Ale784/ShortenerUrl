namespace ShortenerUrl.Entities
{
    public class UrlInput
    {
        
        public string? Id { get; set; }
        public string? ShortUrl { get; set; }
        public string? RedirectUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ExpiratesAt { get; set; }
        public DateTimeOffset LastVisited { get; set; }


      
    }

}