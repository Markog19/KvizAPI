using System;

namespace KvizAPI.Application.DTO
{
    public class QuestionDto
    {
        public Guid Id { get; set; }

        public string? Text { get; set; }

        public string? Answer { get; set; }
    }
}
