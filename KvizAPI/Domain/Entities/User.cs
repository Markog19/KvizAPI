using System;
using System.Collections.Generic;

namespace KvizAPI.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Username { get; set; }

        public string? Email { get; set; }

        // Stored hashed password (do not expose in DTO)
        public string? PasswordHash { get; set; }

        public bool? IsDeleted { get; set; }

        // Navigation: quizzes created by the user
        public List<Quiz>? Quizzes { get; set; }
    }
}
