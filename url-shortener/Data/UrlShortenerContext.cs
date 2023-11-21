using Microsoft.EntityFrameworkCore;
using url_shortener.Data.Entities;
using url_shortener.Helpers;

namespace url_shortener.Data
{
    public class UrlShortenerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Url> Urls { get; set; }
        public DbSet<Category> Categories { get; set; }
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
            Category category = new Category()
            {
                Id = 1,
                Name = "testcat",
                UserId = 1,
            };
            Url url = new Url()
            {
                Id = 1,
                FullUrl = "https://google.com",
                ShortUrl = Shortener.GetShortUrl(),
                Clicks = 0,
                UserId = 1,
                Categories = new List<Category>()
            };
            modelBuilder.Entity<User>(user =>
            {
                user.HasMany(x => x.Categories)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId);
                user.HasMany(x => x.Urls)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId);
                user.HasData(admin);
            });
            modelBuilder.Entity<Category>(c => 
            {
                c.HasOne(x => x.User)
                    .WithMany(user => user.Categories)
                    .HasForeignKey(x => x.UserId);
                c.HasMany(x => x.Urls)
                    .WithMany(x => x.Categories);
                c.HasData(category);
            });
            modelBuilder.Entity<Url>(u =>
            {
                u.HasOne(url => url.User)
                    .WithMany(user => user.Urls)
                    .HasForeignKey(url => url.UserId);
                u.HasMany(url => url.Categories)
                    .WithMany(cat => cat.Urls);
                /*
                    .UsingEntity(i => i
                        .ToTable("UrlCategories")
                    );
                */
                u.HasData(url);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
