using Microsoft.EntityFrameworkCore;
using url_shortener.Data.Entities;
using url_shortener.Helpers;

namespace url_shortener.Data
{
    public class UrlShortenerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Url> Urls { get; set; }
        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> dbContextOptions) : base(dbContextOptions) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User admin = new User()
            {
                Id = 1,
                Username = "Admin",
                Email = "admin@mail.com",
                PasswordHash = PasswordHashing.GetPasswordHash("admin"),
                Role = Models.Eums.RoleEnum.Admin
            };

            Url url = new Url()
            {
                Id = 1,
                FullUrl = "https://google.com",
                ShortUrl = Shortener.GetShortUrl(),
                Clicks = 0,
                UserId = 1
            };

            modelBuilder.Entity<User>(user =>
            {
                user.HasData(admin);
                user.HasMany(us => us.Urls);
            });

            modelBuilder.Entity<Url>(u =>
            {
                u.HasData(url);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
