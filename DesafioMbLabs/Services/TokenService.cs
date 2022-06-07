using DesafioMbLabs.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DesafioMbLabs.Services
{
    public class TokenService
    {
        public static string GenerateTocken(User user, byte[] key)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            var tockenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.Rule.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tocken = tokenHandler.CreateToken(tockenDescriptor);

            return tokenHandler.WriteToken(tocken);
        }
    }
}
