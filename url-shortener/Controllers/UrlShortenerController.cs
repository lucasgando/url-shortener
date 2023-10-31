using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using url_shortener.Data.Entities;
using url_shortener.Data.Models.Dtos;
using url_shortener.Services;

namespace url_shortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UrlShortenerController : Controller
    {
        private readonly UrlService _service;
        private readonly UserService _userService;
        public UrlShortenerController(UrlService urlService, UserService userService)
        {
            _userService = userService;
            _service = urlService;
        }
        [HttpGet("admin/urls")]
        public IActionResult GetUrls()
        {
            return Ok(_service.GetAll());
        }
        [HttpGet("admin/urls/{id}")]
        public IActionResult GetUrl(int id)
        {
            Url? url = _service.GetById(id);
            if (url != null)
                return Ok(url);
            return NotFound();
        }
        [HttpGet("urls")]
        public IActionResult GetUserUrls()
        {
            int userId = Int32.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("nameidentifier"))!.Value);
            List<Url> urls = _service.GetByUserId(userId);
            return Ok(urls);
        }
        [HttpGet("{code}")]
        [AllowAnonymous]
        public IActionResult RedirectToUrl(string code)
        {
            Url? url = _service.GetByCode(code);
            if (url == null) return NotFound();
            _service.UpdateClicks(url.Id);
            return Redirect(url.FullUrl);
        }
        [HttpPost]
        public IActionResult Shorten([FromBody] UrlForCreationDto url)
        {
            int userId = Int32.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("nameidentifier"))!.Value);
            return Ok(_service.Add(url, userId));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string userRole = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)!.Value;
            int userId = Int32.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("nameidentifier"))!.Value);
            Url? url = _service.GetById(id);
            if (url is null) return NotFound();
            if (userRole != "Admin" || userId != url.UserId) return Forbid();
            return _service.Delete(id) ? Ok() : NotFound();
        }
    }
}
