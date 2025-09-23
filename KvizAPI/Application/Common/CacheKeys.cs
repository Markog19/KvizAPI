using System;

namespace KvizAPI.Application.Common
{
    public static class CacheKeys
    {
        public static string Quizzes(Guid userId) => $"quizzes_{userId}";
        public static string QuizzesWithQuestions(Guid userId) => $"quizzes_with_questions_{userId}";
        public static string QuizWithQuestions(Guid userId, Guid quizId) => $"quiz_with_questions_{userId}_{quizId}";
    }
}
