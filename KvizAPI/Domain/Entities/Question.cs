using System;

namespace KvizAPI.Domain.Entities
{
    public class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Text { get; set; }

        public string? Answer { get; set; }
        public bool? IsDeleted { get; set; }

        public List<QuestionQuiz>? QuestionQuizzes { get; set; }

    }
}
