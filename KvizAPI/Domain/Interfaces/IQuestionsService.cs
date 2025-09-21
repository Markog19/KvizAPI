
using Plugins.DTOs;

namespace KvizAPI.Domain.Interfaces
{
    public interface IQuestionsService
    {
        Task<List<QuestionDto>> GetAllAsync();
        Task<QuestionDto?> GetByIdAsync(Guid id);
        Task<QuestionDto> CreateAsync(QuestionDto question);
        Task UpdateAsync(Guid id, QuestionDto question);
        Task DeleteAsync(Guid id);
        Task<List<QuestionDto>> SearchAsync(string searchString);
    }
}
