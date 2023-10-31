using Microsoft.EntityFrameworkCore;
using url_shortener.Data.Entities;

namespace url_shortener.Data
{
    public class UrlShortenerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Url> Urls { get; set; }
        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> dbContextOptions) : base(dbContextOptions) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(
                    new User()
                    {
                        Id = 1,
                        Username = "elpapu",
                        Email = "elpapu@mail.com",
                        Password = "elpapumisterioso"
                    },
                    new User()
                    {
                        Id = 2,
                        Username = "chimba",
                        Email = "chimba@mail.com",
                        Password = "123456"
                    }
                );
            modelBuilder.Entity<Url>()
                .HasData(
                    new Url()
                    {
                        Id = 1,
                        FullUrl = "https://google.com",
                        ShortUrl = "asiubfga",
                        Clicks = 999999,
                        UserId = 1
                    }
                );
            modelBuilder.Entity<User>()
                .HasMany(us => us.Urls);

            modelBuilder.Entity<Url>()
                .HasOne(url => url.User);

            base.OnModelCreating(modelBuilder);
        }
    }
}
