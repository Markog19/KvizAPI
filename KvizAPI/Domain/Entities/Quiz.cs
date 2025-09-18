using System;
using System.Collections.Generic;

namespace KvizAPI.Domain.Entities
{
    public class Quiz
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Name { get; set; }
        public bool? IsDeleted { get; set; }

        public List<Question>? Questions { get; set; }

        public List<QuestionQuiz>? QuestionQuizzes { get; set; }
    }
}
