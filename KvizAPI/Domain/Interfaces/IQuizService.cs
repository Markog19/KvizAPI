using KvizAPI.Application.DTO;

namespace KvizAPI.Domain.Interfaces
{
    public interface IQuizService
    {
        Task CreateQuizAsync(Guid userId, string quizName, List<QuestionDto> questions);
        Task<List<string>> GetAllQuizzesAsync();
        Task<List<QuizDto>> GetAllQuizzesWithQuestionsAsync();
        Task UpdateQuizAsync(Guid quizId, string newName, List<QuestionDto> updatedQuestions);
        Task DeleteQuizAsync(Guid quizId);
    }
}
