using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using TournamentCalendarAPI.Models;
using TournamentCalendarAPI.Repository;

namespace TournamentCalendarAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private static readonly Dictionary<string, string> refreshTokens = new();

        public AuthController(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (ValidateUserCredentials(user.Username, user.Password, out var role))
            { 
                var token = GenerateJwtToken(user.Username, role);

                return Ok(new
                {
                    Token = token,
                });
            }

            return Unauthorized();
        }

        private bool ValidateUserCredentials(string username, string password, out string role)
        {
            var user =  _userRepository.GetUser(username, password).Result;
            if (user == null)
            {
                role = "unathorized";
                return false;
            }
            role = user.Role;
            return true;

        }
        private string GenerateJwtToken(string username, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role) // Dodanie roli
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
