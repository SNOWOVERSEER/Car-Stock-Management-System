using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CarStockManagementAPI.Utils
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string dealerId);
    }
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private string _secretKey;
        public JwtTokenGenerator(string secretKey)
        {
            _secretKey = secretKey;
        }
        public string GenerateToken(string dealerId)
        {
            var token = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, dealerId)
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenGenerated = token.CreateToken(tokenDescriptor);
            return token.WriteToken(tokenGenerated);
        }

    }
}