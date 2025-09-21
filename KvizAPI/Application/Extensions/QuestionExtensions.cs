using KvizAPI.Domain.Entities;
using Plugins.DTOs;

namespace KvizAPI.Application.Extensions
{
    public static class QuestionExtensions
    {
        public static QuestionDto ToDto(this Question? question)
        {
            if (question == null) return null!;

            return new QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                Answer = question.Answer
            };
        }

        public static List<QuestionDto> ToDtoList(this IEnumerable<Question>? questions)
        {
            if (questions == null)
            {
                return [];
            }
            return questions.Select(q => q.ToDto()).ToList();
        }
    }
}
