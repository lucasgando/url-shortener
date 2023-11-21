using Microsoft.AspNetCore.Mvc;
using url_shortener.Data.Implementations;
using url_shortener.Data.Models.Dtos.Url;
using url_shortener.Services;

namespace url_shortener.Controllers
{
    [ApiController]
    [Route("")]
    public class RedirectController : BaseController
    {
        private readonly UrlService _service;
        public RedirectController(UrlService service) { _service = service; }
        [HttpGet("{code}")]
        public IActionResult RedirectToUrl(string code)
        {
            UrlDto? url = _service.GetByCode(code);
            if (url is null) return NotFound();
            _service.UpdateClicks(url.Id);
            return Redirect(url.FullUrl);
        }
    }
}
