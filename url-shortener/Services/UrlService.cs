using url_shortener.Data;
using url_shortener.Data.Entities;
using url_shortener.Data.Models.Dtos;
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
        public List<Url> GetAll()
        {
            return _context.Urls.ToList();
        }
        public List<Url> GetByUserId(int id)
        {
            return _context.Urls.Where(url => url.UserId == id).ToList();
        }
        public Url? GetById(int id)
        {
            return _context.Urls.SingleOrDefault(url => url.Id == id);
        }
        public Url? GetByCode(string shortUrl)
        {
            return _context.Urls.SingleOrDefault(url => url.ShortUrl == shortUrl);
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
        public bool Delete(int id)
        {
            Url? urlToDel = GetById(id);
            if (urlToDel == null) return false;
            _context.Urls.Remove(urlToDel);
            _context.SaveChanges();
            return true;
        }
        public bool UpdateClicks(int id)
        {
            Url? urlToUpd = GetById(id);
            if (urlToUpd == null) return false;
            urlToUpd.Clicks++;
            _context.Urls.Update(urlToUpd);
            _context.SaveChanges();
            return true;
            
        }
        public bool Update(int id)
        { 
            Url? urlToUpd = GetById(id);
            if (urlToUpd != null)
            {
                urlToUpd.ShortUrl = Shortener.GetShortUrl();
                _context.Urls.Update(urlToUpd);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
