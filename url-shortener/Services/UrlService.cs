using url_shortener.Data;
using url_shortener.Data.Entities;
using url_shortener.Data.Models;
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
        public List<Url> GetUrls()
        {
            return _context.Urls.ToList();
        }
        public Url? GetUrl(int id)
        {
            return _context.Urls.SingleOrDefault(url => url.Id == id);
        }
        public Url? GetUrlByCode(string shortUrl)
        {
            return _context.Urls.SingleOrDefault(url => url.ShortUrl == shortUrl);
        }
        public int AddUrl(UrlDto url)
        {
            Url newUrl = new Url()
            {
                FullUrl = url.FullUrl,
                ShortUrl = Shortener.GetShortUrl(),
                UserId = url.UserId
            };
            _context.Urls.Add(newUrl);
            _context.SaveChanges();
            return newUrl.Id;
        }
        public bool DelUrl(int id)
        {
            Url? urlToDel = GetUrl(id);
            if (urlToDel != null)
            {
                _context.Urls.Remove(urlToDel);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool UpdateClicks(int id)
        {
            Url? urlToUpd = GetUrl(id);
            if (urlToUpd != null)
            {
                urlToUpd.Clicks++;
                _context.Urls.Update(urlToUpd);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool UpdateShortUrl(int id)
        { 
            Url? urlToUpd = GetUrl(id);
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
