using Xunit;
using KvizAPI.Application.Services;
using KvizAPI.Infrastructure.DBContexts;
using KvizAPI.Application.DTO;
using KvizAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Plugins.DTOs;

namespace KvizApi.Tests.Services
{
    public class QuestionsServiceTests
    {
        private static QuizDbContext GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<QuizDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var db = new QuizDbContext(options);
            db.Questions.Add(new Question { Id = Guid.NewGuid(), Text = "Q1", Answer = "A1" });
            db.Questions.Add(new Question { Id = Guid.NewGuid(), Text = "Q2", Answer = "A2" });
            db.SaveChanges();
            return db;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllQuestions()
        {
            var db = GetDbContextWithData();
            var service = new QuestionsService(db);
            var result = await service.GetAllAsync();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task CreateAsync_AddsQuestion()
        {
            var db = GetDbContextWithData();
            var service = new QuestionsService(db);
            var dto = new QuestionDto { Text = "Q3", Answer = "A3" };
            var created = await service.CreateAsync(dto);
            Assert.NotNull(created);
            Assert.Equal("Q3", created.Text);
            Assert.Equal("A3", created.Answer);
        }

        [Fact]
        public async Task DeleteAsync_SoftDeletesQuestion()
        {
            var db = GetDbContextWithData();
            var service = new QuestionsService(db);
            var q = db.Questions.First();
            await service.DeleteAsync(q.Id);
            var deleted = db.Questions.Find(q.Id);
            Assert.True(deleted.IsDeleted);
        }
    }
}
