using KvizAPI.Application.DTO;
using Plugins.DTOs;

namespace KvizAPI.Domain.Interfaces
{
    public interface IQuizService
    {
        Task CreateQuizAsync(Guid userId, string quizName, List<QuestionDto> questions);
        Task<List<QuizDto>> GetAllQuizzesAsync(Guid userId);
        Task<List<QuizDto>> GetAllQuizzesWithQuestionsAsync(Guid userId);
        Task UpdateQuizAsync(Guid quizId, string newName, List<QuestionDto> updatedQuestions);
        Task DeleteQuizAsync(Guid quizId);
        Task<QuizDto> GetQuizzWithQuestionsAsync(Guid userId, Guid quizId);

    }
}
