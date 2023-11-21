using Microsoft.EntityFrameworkCore;
using url_shortener.Data;
using url_shortener.Data.Entities;
using url_shortener.Data.Models.Dtos.Url;
using url_shortener.Helpers;

namespace url_shortener.Services
{
    public class UrlService
    {
        private readonly UrlShortenerContext _context;
        public UrlService(UrlShortenerContext context)
        {
            _context = context;
        }
        public List<UrlDto> GetAll()
        {
            return _context.Urls.Include(x => x.Categories)
                .Select(url => DtoHandler.GetUrlDto(url)).ToList();
        }
        public List<UrlDto> GetByUserId(int id)
        {
            return _context.Urls.Where(url => url.UserId == id).Include(x => x.Categories)
                .Select(x => DtoHandler.GetUrlDto(x)).ToList();
        }
        public List<UrlDto> GetByCategory(string categoryName)
        {
            return _context.Categories.Include(x => x.Urls).First(x => x.Name == categoryName).Urls
                .Select(x => DtoHandler.GetUrlDto(x)).ToList();
        }
        public UrlDto? GetById(int id)
        {
            Url? url = _context.Urls.SingleOrDefault(url => url.Id == id);
            return url is null ? null : DtoHandler.GetUrlDto(url);
        }
        public UrlDto? GetByCode(string shortUrl)
        {
            Url? url = _context.Urls.SingleOrDefault(url => url.ShortUrl == shortUrl);
            return url is null ? null : DtoHandler.GetUrlDto(url);
        }
        public bool UserIsOwner(int userId, int urlId)
        {
            UrlDto? url = GetById(urlId);
            return url is not null && userId == url.UserId;
        }
        public int Add(UrlForCreationDto url, int userId)
        {
            Url newUrl = new Url()
            {
                FullUrl = url.Url,
                ShortUrl = Shortener.GetShortUrl(),
                UserId = userId
            };
            _context.Urls.Add(newUrl);
            _context.SaveChanges();
            return newUrl.Id;
        }
        public void ChangeCategories(UrlForUpdateDto url)
        {
            Url dbUrl = _context.Urls.Single(x => x.Id == url.Id);
            int userId = dbUrl.UserId;
            foreach (int catId in url.NewCategories)
            {
                Category? cat = _context.Categories.Include(x => x.Urls).FirstOrDefault(x => x.Id == catId);
                if (cat is not null && userId == cat.UserId && !cat.Urls.Any(x => x.Id == url.Id))
                {
                    dbUrl.Categories.Add(cat);
                    _context.Urls.Update(dbUrl);
                    cat.Urls.Add(dbUrl);
                    _context.Categories.Update(cat);
                }
            }
            foreach (int catId in url.DeleteCategories)
            {
                Category? cat = _context.Categories.Include(x => x.Urls).FirstOrDefault(x => x.Id == catId);
                if (cat is not null && userId == cat.UserId)
                {
                    dbUrl.Categories.Remove(cat);
                    _context.Urls.Update(dbUrl);
                    cat.Urls.Remove(dbUrl);
                    _context.Categories.Update(cat);
                }
            }
            _context.SaveChanges();
        }
        public void UpdateClicks(int id)
        {
            Url? urlToUpd = _context.Urls.First(x => x.Id == id);
            urlToUpd.Clicks++;
            _context.Urls.Update(urlToUpd);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            Url urlToDel = _context.Urls.First(x => x.Id == id);
            foreach (Category cat in urlToDel.Categories)
            {
                cat.Urls.Remove(urlToDel);
                _context.Categories.Update(cat);
            }
            _context.Urls.Remove(urlToDel);
            _context.SaveChanges();
        }
    }
}
