using System;

namespace KvizAPI.Domain.Entities
{
    public class QuestionQuiz
    {
        // Removed surrogate Id to use composite key (QuestionId, QuizId)
        public Guid QuestionId { get; set; }
        public Question? Question { get; set; }

        public Guid QuizId { get; set; }
        public Quiz? Quiz { get; set; }

    }
}
