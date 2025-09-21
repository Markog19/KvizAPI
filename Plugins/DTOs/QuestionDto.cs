using System;

namespace Plugins.DTOs
{
    public class QuestionDto
    {
        public Guid Id { get; set; }

        public string? Text { get; set; }

        public string? Answer { get; set; }
    }
}
