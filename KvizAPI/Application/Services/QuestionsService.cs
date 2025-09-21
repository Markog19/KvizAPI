using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KvizAPI.Application.DTO;
using KvizAPI.Domain.Entities;
using KvizAPI.Domain.Interfaces;
using KvizAPI.Infrastructure.DBContexts;
using KvizAPI.Application.Extensions;

namespace KvizAPI.Application.Services
{
    public class QuestionsService(QuizDbContext db) : IQuestionsService
    {
       

        public async Task<List<QuestionDto>> GetAllAsync()
        {
            var questions = await db.Questions.ToListAsync();
            return questions.ToDtoList();
        }

        public async Task<QuestionDto?> GetByIdAsync(Guid id)
        {
            var question = await db.Questions.FindAsync(id);
            return question?.ToDto();
        }

        public async Task<QuestionDto> CreateAsync(QuestionDto questionDto)
        {
            var question = new Question
            {
                Text = questionDto.Text,
                Answer = questionDto.Answer,
                IsDeleted = false
            };
            db.Questions.Add(question);
            await db.SaveChangesAsync();
            return question.ToDto();
        }

        public async Task UpdateAsync(Guid id, QuestionDto questionDto)
        {
            var question = await db.Questions.FindAsync(id);
            if (question == null) return;
            question.Text = questionDto.Text;
            question.Answer = questionDto.Answer;
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var question = await db.Questions.FindAsync(id);
            if (question == null) return;
            question.IsDeleted = true;
            await db.SaveChangesAsync();
        }

        public async Task<List<QuestionDto>> SearchAsync(string searchString)
        {
            var questions = await db.Questions
                .Where(q => q.Text != null && q.Text.Contains(searchString))
                .ToListAsync();
            return questions.ToDtoList();
        }
    }
}
