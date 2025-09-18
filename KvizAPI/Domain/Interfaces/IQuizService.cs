using KvizAPI.Application.DTO;

namespace KvizAPI.Domain.Interfaces
{
    public interface IQuizService
    {
        public Task CreateQuizAsync(Guid userId, string quizName, List<QuestionDto> questions);
        public Task<List<string>> GetAllQuizzesAsync();
        public Task<List<QuizDto>> GetAllQuizzesWithQuestionsAsync();

        public Task<QuizDto?> GetQuizByIdAsync(Guid quizId);
        public Task UpdateQuizAsync(Guid quizId, string newName, List<QuestionDto> updatedQuestions);
        public Task DeleteQuizAsync(Guid quizId);
        public Task<List<QuestionDto>> GetQuestions(string searchString);
    }
}
