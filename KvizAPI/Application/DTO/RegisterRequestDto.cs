using System;

namespace KvizAPI.Application.DTO
{
    public class RegisterRequestDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
