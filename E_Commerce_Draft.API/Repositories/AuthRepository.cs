using E_Commerce_Draft.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce_Draft.API.Repositories
{
    public class AuthRepository: IAuthRepository
    {
        private readonly IConfiguration configuration;

        public AuthRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> GenerateJwtTokenAsync(string username, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ValidateUserAsync(UserLoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "admin")
            {
                return true;
            }

            if (request.Username == "user" && request.Password == "user")
            {
                return true;
            }

            return false;

        }
    }
}
