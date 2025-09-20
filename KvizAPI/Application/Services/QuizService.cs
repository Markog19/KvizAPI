
using Microsoft.EntityFrameworkCore;
using KvizAPI.Application.DTO;
using KvizAPI.Domain.Interfaces;
using KvizAPI.Infrastructure.DBContexts;
using KvizAPI.Domain.Entities;
using KvizAPI.Application.Extensions;

namespace KvizAPI.Application.Services
{
    public class QuizService(QuizDbContext quizDbContext) : IQuizService
    {
        public Task CreateQuizAsync(Guid userId, string quizName, List<QuestionDto> questions)
        {
            if(questions == null || questions.Count == 0)
            {
                questions = quizDbContext.Questions.Take(20).ToList().ToDtoList();
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

        public Task<List<string>> GetAllQuizzesAsync()
        {
            return quizDbContext.Quizzes
                .Select(q => q.Name)
                .ToListAsync();
        }

        public Task<List<QuizDto>> GetAllQuizzesWithQuestionsAsync()
        {
            return quizDbContext.Quizzes
                .Include(q => q.QuestionQuizzes.Where(qq => qq.IsDeleted == false))
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

        public Task<List<QuestionDto>> GetQuestions(string searchString)
        {
           return quizDbContext.Questions
                .Where(q => q.Text.Contains(searchString))
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Answer = q.Answer
                })
                .ToListAsync();
        }

     
        public async Task UpdateQuizAsync(Guid quizId, string newName, List<QuestionDto> updatedQuestions)
        {
            var transaction = await quizDbContext.Database.BeginTransactionAsync();

            try
            {
                var quiz = await quizDbContext.Quizzes
                    .FindAsync(quizId);

                if (quiz != null)
                {
                    quiz.Questions = [.. updatedQuestions.Select(q => new Question
                {
                    Text = q.Text,
                    IsDeleted = false,
                    Answer = q.Answer
                })];
                    quiz.Name = newName;
                }
                await quizDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }


        }
    }
}
