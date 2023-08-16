using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Sercurity.JWT
{
    public class JWTUtil
    {
        private readonly IConfiguration _configuration;
        public JWTUtil(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string organizationId, string organizationName, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, organizationId), // Replace userId with your user's unique identifier
                new Claim(ClaimTypes.Name, organizationName), // Replace username with the user's name
                new Claim(ClaimTypes.Role, role) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); // Replace with your secret key
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer, // Replace with your JWT issuer
                audience: audience, // Replace with your JWT audience
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Set token expiration time as needed
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
