using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Sercurity.JWT
{
    public class JwtTokenValidator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsTokenValid(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), // Convert the secret key from base64
                ValidateIssuer = true,
                ValidIssuer = issuer, // Replace with the issuer used during token generation
                ValidateAudience = true,
                ValidAudience = audience, // Replace with the audience used during token generation
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Optional: Adjust the clock skew to accommodate small time differences between server and client
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
