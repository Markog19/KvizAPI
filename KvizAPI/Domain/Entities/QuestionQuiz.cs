using System;

namespace KvizAPI.Domain.Entities
{
    public class QuestionQuiz
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid QuestionId { get; set; }
        public Question? Question { get; set; }

        public Guid QuizId { get; set; }
        public Quiz? Quiz { get; set; }

    }
}
