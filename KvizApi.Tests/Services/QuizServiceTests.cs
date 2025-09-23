using Xunit;
using KvizAPI.Application.Services;
using KvizAPI.Infrastructure.DBContexts;
using KvizAPI.Application.DTO;
using KvizAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory; // Add this using directive
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Plugins.DTOs;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace KvizApi.Tests.Services
{
    public class QuizServiceTests
    {
        private static QuizDbContext GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<QuizDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            var db = new QuizDbContext(options);
            db.Questions.Add(new Question { Id = Guid.NewGuid(), Text = "Q1", Answer = "A1" });
            db.Questions.Add(new Question { Id = Guid.NewGuid(), Text = "Q2", Answer = "A2" });
            db.SaveChanges();
            return db;
        }

        [Fact]
        public async Task CreateQuizAsync_CreatesQuizWithQuestions()
        {
            var db = GetDbContextWithData();
            var service = new QuizService(db);
            var userId = Guid.NewGuid();
            var quizName = "TestQuiz";
            var questions = db.Questions.Select(q => new QuestionDto { Text = q.Text, Answer = q.Answer }).ToList();

            await service.CreateQuizAsync(userId, quizName, questions);

            var quiz = db.Quizzes.Include(q => q.QuestionQuizzes).ThenInclude(qq => qq.Question).FirstOrDefault(q => q.Name == quizName);
            Assert.NotNull(quiz);
            Assert.Equal(userId, quiz.UserId);
            Assert.Equal(questions.Count, quiz.QuestionQuizzes.Count);
        }

        [Fact]
        public async Task GetAllQuizzesAsync_ReturnsUserQuizzes()
        {
            var db = GetDbContextWithData();
            var service = new QuizService(db);
            var userId = Guid.NewGuid();
            db.Quizzes.Add(new Quiz { Id = Guid.NewGuid(), Name = "Quiz1", UserId = userId });
            db.Quizzes.Add(new Quiz { Id = Guid.NewGuid(), Name = "Quiz2", UserId = userId });
            db.SaveChanges();

            var result = await service.GetAllQuizzesAsync(userId);

            Assert.Equal(2, result.Count);
            Assert.All(result, q => Assert.Equal(userId, db.Quizzes.First(x => x.Id == q.Id).UserId));
        }

        [Fact]
        public async Task DeleteQuizAsync_SoftDeletesQuizAndQuestions()
        {
            var db = GetDbContextWithData();
            var service = new QuizService(db);
            var userId = Guid.NewGuid();
            var quiz = new Quiz { Id = Guid.NewGuid(), Name = "QuizDel", UserId = userId, IsDeleted = false };
            db.Quizzes.Add(quiz);
            db.SaveChanges();

            await service.DeleteQuizAsync(quiz.Id);

            var deletedQuiz = db.Quizzes.Find(quiz.Id);
            Assert.True(deletedQuiz.IsDeleted);
        }
    }
}
