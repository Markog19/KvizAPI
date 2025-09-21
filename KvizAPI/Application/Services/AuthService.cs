using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using KvizAPI.Application.DTO;
using KvizAPI.Domain.Entities;
using KvizAPI.Infrastructure.DBContexts;
using KvizAPI.Application.Common.Security;
using Microsoft.Extensions.Options;
using KvizAPI.Domain.Interfaces;

namespace KvizAPI.Application.Services
{
    public class AuthService(QuizDbContext db, IOptions<AuthOptions> options) : IAuthService
    {
        public async Task<AuthResultDto> RegisterAsync(RegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthResultDto { Success = false, Error = "Username and password are required" };
            }
            var exists = await db.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email);
            if (exists)
            {
                return new AuthResultDto { Success = false, Error = "User already exists" };
            }

            var hash = PasswordHelper.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hash,
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            var token = await GenerateTokenAsync(user);
            return new AuthResultDto
            {
                Success = true,
                Token = token.Token,
                ExpiresAt = token.ExpiresAt
            };
        }

       

        public async Task<AuthResultDto> LoginAsync(LoginRequestDto request)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !PasswordHelper.Verify(user.PasswordHash, request.Password))
            {
                return new AuthResultDto { Success = false, Error = "Invalid credentials" };
            }

            var token = await GenerateTokenAsync(user);
            return new AuthResultDto
            {
                Success = true,
                Token = token.Token,
                ExpiresAt = token.ExpiresAt
            };
        }

        private Task<AuthResponseDto> GenerateTokenAsync(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username ?? string.Empty)
            };

            var expires = DateTime.UtcNow.AddHours(6);
            var token = new JwtSecurityToken(options.Value.Issuer, options.Value.Audience, claims, expires: expires, signingCredentials: creds);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Task.FromResult(new AuthResponseDto { Token = tokenStr, ExpiresAt = expires });
        }
    }
}
