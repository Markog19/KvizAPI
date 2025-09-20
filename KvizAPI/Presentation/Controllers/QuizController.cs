using Microsoft.AspNetCore.Mvc;
using KvizAPI.Application.DTO;
using KvizAPI.Domain.Interfaces;

namespace KvizAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController(IQuizService quizService) : ControllerBase
    {
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetAll()
        {
            var quizzes = await quizService.GetAllQuizzesAsync();
            return Ok(quizzes);
        }

       

        // POST: api/quiz
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] QuizDto quiz)
        {
            if (quiz == null) return BadRequest();

            await quizService.CreateQuizAsync(quiz.Id, quiz.Name ?? string.Empty, quiz.Questions ?? new List<QuestionDto>());
            return CreatedAtAction(nameof(Create), new { id = quiz.Id }, null);
        }

        // PUT: api/quiz/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] QuizDto quiz)
        {
            if (quiz == null) return BadRequest();
            await quizService.UpdateQuizAsync(id, quiz.Name ?? string.Empty, quiz.Questions ?? new List<QuestionDto>());
            return Ok();
        }

        // DELETE: api/quiz/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await quizService.DeleteQuizAsync(id);
            return Ok();
        }
    }
}
