using Microsoft.AspNetCore.Mvc;
using url_shortener.Data.Entities;
using url_shortener.Data.Models;
using url_shortener.Services;

namespace url_shortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController : Controller
    {
        private readonly UrlService _service;
        public UrlShortenerController(UrlService urlService)
        {
            _service = urlService;
        }
        [HttpGet("urls")]
        public IActionResult GetUrls()
        {
            return Ok(_service.GetUrls());
        }
        [HttpGet("urls/{id}")]
        public IActionResult GetUrl(int id)
        {
            Url? url = _service.GetUrl(id);
            if (url != null)
                return Ok(url);
            return NotFound();

        }
        [HttpGet("{code}")]
        public IActionResult ShortRedirect(string code)
        {
            Url? url = _service.GetUrlByCode(code);
            if (url != null)
            {
                _service.UpdateClicks(url.Id);
                return Redirect(url.FullUrl);
            }
            return NotFound();
        }
        [HttpPost("shortener/shorten")]
        public IActionResult ShortenUrl([FromBody] UrlDto url)
        {
            return Ok(_service.AddUrl(url));
        }
        [HttpDelete("del-url/{id}")]
        public IActionResult DeleteUrl(int id)
        {
            if (_service.DelUrl(id))
                return Ok();
            return NotFound();
        }
    }
}
