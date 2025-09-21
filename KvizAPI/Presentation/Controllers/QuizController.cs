using Microsoft.AspNetCore.Mvc;
using KvizAPI.Application.DTO;
using KvizAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace KvizAPI.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController(IQuizService quizService) : ControllerBase
    {
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetAll()
        {
            (bool flowControl, ActionResult value) = GetCurrentUser(out Guid userId);
            if (!flowControl)
            {
                return value;
            }
            var quizzes = await quizService.GetAllQuizzesAsync(userId);
            return Ok(quizzes);
        }

        [HttpGet("AllWithQuestions")]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetAllWithQuestions()
        {
            (bool flowControl, ActionResult value) = GetCurrentUser(out Guid userId);
            if (!flowControl)
            {
                return value;
            }
            var quizzes = await quizService.GetAllQuizzesWithQuestionsAsync(userId);
            return Ok(quizzes);
        }
        [HttpGet("OneWithQuestions/{quizId}")]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetOneWithQuestions(Guid quizId)
        {
            (bool flowControl, ActionResult value) = GetCurrentUser(out Guid userId);
            if (!flowControl)
            {
                return value;
            }
            var quizzes = await quizService.GetQuizzWithQuestionsAsync(userId,quizId);
            return Ok(quizzes);
        }

        // POST: api/quiz
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] QuizDto quiz)
        {
            if (quiz == null)
            {
                return BadRequest();
            }
            (bool flowControl, ActionResult value) = GetCurrentUser(out Guid userId);
            if (!flowControl)
            {
                return value;
            }
            await quizService.CreateQuizAsync(userId, quiz.Name ?? string.Empty, quiz.Questions ?? new List<QuestionDto>());
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
        private (bool flowControl, ActionResult value) GetCurrentUser(out Guid userId)
        {
            var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(idClaim, out userId))
            {
                return (flowControl: false, value: Unauthorized());
            }

            return (flowControl: true, value: null);
        }

    }
}
