using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KvizAPI.Application.DTO;
using KvizAPI.Domain.Interfaces;
using KvizAPI.Infrastructure.DBContexts;

namespace KvizAPI.Application.Services
{
    public class QuizService(QuizDbContext quizDbContext) : IQuizService
    {
        public Task CreateQuizAsync(Guid userId, string quizName, List<QuestionDto> questions)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteQuizAsync(Guid quizId)
        {
            var quiz = await quizDbContext.Quizzes.FindAsync(quizId);
            if (quiz != null)
            {
                await using var transaction = await quizDbContext.Database.BeginTransactionAsync();
                try
                {
                    (await quizDbContext.QuestionQuizzes
                        .Where(qt => qt.QuizId == quizId)
                        .ToListAsync())
                        .ForEach(qt => qt.IsDeleted = true);
                   
                    quiz.IsDeleted = true;

                    await quizDbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public Task<List<string>> GetAllQuizzesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<QuizDto>> GetAllQuizzesWithQuestionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<QuestionDto>> GetQuestions(string searchString)
        {
            throw new NotImplementedException();
        }

        public Task<QuizDto?> GetQuizByIdAsync(Guid quizId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateQuizAsync(Guid quizId, string newName, List<QuestionDto> updatedQuestions)
        {
            throw new NotImplementedException();
        }
    }
}
