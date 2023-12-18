using Microsoft.EntityFrameworkCore;

namespace ShortenerUrl.Entities
{
    public class ShortenerDb : DbContext
    {
        public ShortenerDb(DbContextOptions<ShortenerDb> options) : base(options) {}

        public DbSet<UrlInput> Urls => Set<UrlInput>();

    }
}