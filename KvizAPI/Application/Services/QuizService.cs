
using Microsoft.EntityFrameworkCore;
using KvizAPI.Application.DTO;
using KvizAPI.Domain.Interfaces;
using KvizAPI.Infrastructure.DBContexts;
using KvizAPI.Domain.Entities;
using KvizAPI.Application.Extensions;
using Plugins.DTOs;

namespace KvizAPI.Application.Services
{
    public class QuizService(QuizDbContext quizDbContext) : IQuizService
    {


        public Task CreateQuizAsync(Guid userId, string quizName, List<QuestionDto> questions)
        {
            if(questions == null || questions.Count == 0)
            {
                questions = quizDbContext.Questions
                    .OrderBy(q => Guid.NewGuid())
                    .Take(20)
                    .ToList()
                    .ToDtoList();
            }
            
            var quiz = new Quiz
            {
                Name = quizName,
                UserId = userId,
                IsDeleted = false,
                QuestionQuizzes = [.. questions.Select(q => new QuestionQuiz
                {
                    Question = new Question
                    {
                        Text = q.Text,
                        IsDeleted = false,
                        Answer = q.Answer
                    },
                    IsDeleted = false
                })]
            };
            quizDbContext.Quizzes.Add(quiz);
            return quizDbContext.SaveChangesAsync();
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

        public Task<List<QuizDto>> GetAllQuizzesAsync(Guid userId)
        {
            return quizDbContext.Quizzes.Where(q => q.UserId == userId)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Name = q.Name
                })
                .ToListAsync();
        }

        public Task<List<QuizDto>> GetAllQuizzesWithQuestionsAsync(Guid userId)
        {
            return quizDbContext.Quizzes.Where(q => q.UserId == userId)
                .Include(q => q.QuestionQuizzes)
                    .ThenInclude(qq => qq.Question)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Name = q.Name,
                    Questions = q.QuestionQuizzes
                        .Select(qq => new QuestionDto
                        {
                            Id = qq.Question.Id,
                            Text = qq.Question.Text,
                            Answer = qq.Question.Answer
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<QuizDto> GetQuizzWithQuestionsAsync(Guid userId, Guid quizId)
        {
            return await quizDbContext.Quizzes
                .Where(q => q.UserId == userId)
                .Where(q => q.Id == quizId)
                .Include(q => q.QuestionQuizzes)
                    .ThenInclude(qq => qq.Question)
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Name = q.Name,
                    Questions = q.QuestionQuizzes
                        .Select(qq => new QuestionDto
                        {
                            Id = qq.Question.Id,
                            Text = qq.Question.Text,
                            Answer = qq.Question.Answer
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdateQuizAsync(Guid quizId, string newName, List<QuestionDto> updatedQuestions)
        {
            await using var transaction = await quizDbContext.Database.BeginTransactionAsync();

            try
            {
                var quiz = await quizDbContext.Quizzes
                    .Include(q => q.QuestionQuizzes)
                    .ThenInclude(qq => qq.Question)
                    .FirstOrDefaultAsync(q => q.Id == quizId);

                if (quiz != null)
                {
                    quiz.Name = newName;

                    quizDbContext.QuestionQuizzes.RemoveRange(quiz.QuestionQuizzes);

                    await quizDbContext.SaveChangesAsync();

                    quiz.QuestionQuizzes = updatedQuestions.Select(q => new QuestionQuiz
                    {
                        QuizId = quiz.Id,
                        Question = new Question
                        {
                            Text = q.Text,
                            Answer = q.Answer,
                            IsDeleted = false
                        },
                        IsDeleted = false
                    }).ToList();

                    quizDbContext.QuestionQuizzes.AddRange(quiz.QuestionQuizzes);

                    await quizDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
    
}
