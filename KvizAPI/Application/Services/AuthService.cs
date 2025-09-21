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

namespace KvizAPI.Application.Services
{
    public class AuthService
    {
        private readonly QuizDbContext _db;
        private readonly AuthOptions _options;

        public AuthService(QuizDbContext db, AuthOptions options)
        {
            _db = db;
            _options = options;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Username and password are required");

            // check if user exists
            var exists = await _db.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email);
            if (exists) throw new InvalidOperationException("User already exists");

            // hash password - for demo use simple SHA256; replace with a proper password hasher in production
            using var sha = System.Security.Cryptography.SHA256.Create();
            var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(request.Password ?? string.Empty)));

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hash,
                IsDeleted = false
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return await GenerateTokenAsync(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null) throw new InvalidOperationException("Invalid credentials");

            using var sha = System.Security.Cryptography.SHA256.Create();
            var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(request.Password ?? string.Empty)));

            if (user.PasswordHash != hash) throw new InvalidOperationException("Invalid credentials");

            return await GenerateTokenAsync(user);
        }

        private Task<AuthResponseDto> GenerateTokenAsync(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username ?? string.Empty)
            };

            var expires = DateTime.UtcNow.AddHours(6);
            var token = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, expires: expires, signingCredentials: creds);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Task.FromResult(new AuthResponseDto { Token = tokenStr, ExpiresAt = expires });
        }
    }
}
