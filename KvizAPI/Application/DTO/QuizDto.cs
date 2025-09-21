using System;
using System.Collections.Generic;

namespace KvizAPI.Application.DTO
{
    public class QuizDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }


        public List<QuestionDto>? Questions { get; set; } = new List<QuestionDto>();
    }
}
