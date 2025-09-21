using System;

namespace KvizAPI.Application.DTO
{
    public class AuthResponseDto
    {
        public string? Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
