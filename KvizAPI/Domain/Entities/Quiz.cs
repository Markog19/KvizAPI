using System;
using System.Collections.Generic;

namespace KvizAPI.Domain.Entities
{
    public class Quiz
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public List<Question>? Questions { get; set; }

        public List<QuestionQuiz>? QuestionQuizzes { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
