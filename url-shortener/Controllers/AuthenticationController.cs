﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using url_shortener.Data.Models.Dtos;
using url_shortener.Data.Models.Dtos.User;
using url_shortener.Services;

namespace url_shortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        public readonly IConfiguration _config;
        public readonly UserService _service;
        public AuthenticationController(IConfiguration config, UserService service)
        {
            _config = config;
            _service = service;
        }
        [HttpPost]
        public IActionResult Authenticate([FromBody] CredentialsDto dto)
        {
            UserDto? user = _service.Get(dto.Email);
            if (user is null) return Unauthorized();
            if(!_service.Authenticate(dto.Email, dto.Password)) return Unauthorized();

            SymmetricSecurityKey securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]!));
            SigningCredentials credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256Signature);

            List<Claim> claimsForToken = new List<Claim>();
            // standard claim names
            claimsForToken.Add(new Claim("sub", user.Id.ToString())); // sub == nameidentifier
            claimsForToken.Add(new Claim("given_email", user.Email));
            claimsForToken.Add(new Claim("role", user.Role.ToString()));

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                _config["Authentication:Issuer"],
                _config["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1), // token valid for one hour
                credentials
            );

            string tokenToReturn = new JwtSecurityTokenHandler() // stringify token
                .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }
    }
}
