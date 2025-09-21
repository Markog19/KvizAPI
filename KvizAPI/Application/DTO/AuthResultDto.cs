using System;

namespace KvizAPI.Application.DTO
{
    public class AuthResultDto
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? Error { get; set; }
    }
}
