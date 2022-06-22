using TicketsManager.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TicketsManager.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateTocken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            var key = Encoding.ASCII.GetBytes(_configuration["JwtSecret"]);
            var tockenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tocken = tokenHandler.CreateToken(tockenDescriptor);

            return tokenHandler.WriteToken(tocken);
        }
    }
}
