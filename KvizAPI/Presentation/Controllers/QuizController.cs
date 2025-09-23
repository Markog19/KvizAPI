using Microsoft.AspNetCore.Mvc;
using KvizAPI.Application.DTO;
using KvizAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Plugins.DTOs;
using Microsoft.Extensions.Caching.Memory;
using KvizAPI.Application.Common;
using Microsoft.Extensions.Options;

namespace KvizAPI.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController(IQuizService quizService, IMemoryCache cache, IOptions<CacheSettings> cacheSettings) : ControllerBase
    {
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetAll()
        {
            (bool flowControl, ActionResult value) = GetCurrentUser(out Guid userId);
            if (!flowControl)
            {
                return value;
            }
            var cacheKey = CacheKeys.Quizzes(userId);
            if (!cache.TryGetValue(cacheKey, out IEnumerable<QuizDto> quizzes))
            {
                quizzes = await quizService.GetAllQuizzesAsync(userId);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(cacheSettings.Value.QuizzesExpirationMinutes));
                cache.Set(cacheKey, quizzes, cacheEntryOptions);
            }
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
            var cacheKey = CacheKeys.QuizzesWithQuestions(userId);
            if (!cache.TryGetValue(cacheKey, out IEnumerable<QuizDto> quizzes))
            {
                quizzes = await quizService.GetAllQuizzesWithQuestionsAsync(userId);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(cacheSettings.Value.QuizzesWithQuestionsExpirationMinutes));
                cache.Set(cacheKey, quizzes, cacheEntryOptions);
            }
            return Ok(quizzes);
        }

        [HttpGet("OneWithQuestions/{quizId}")]
        public async Task<ActionResult<QuizDto>> GetOneWithQuestions(Guid quizId)
        {
            (bool flowControl, ActionResult value) = GetCurrentUser(out Guid userId);
            if (!flowControl)
            {
                return value;
            }
            var cacheKey = CacheKeys.QuizWithQuestions(userId, quizId);
            if (!cache.TryGetValue(cacheKey, out QuizDto quiz))
            {
                quiz = await quizService.GetQuizzWithQuestionsAsync(userId, quizId);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(cacheSettings.Value.QuizWithQuestionsExpirationMinutes));
                cache.Set(cacheKey, quiz, cacheEntryOptions);
            }
            return Ok(quiz);
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
            cache.Remove(CacheKeys.Quizzes(userId));
            cache.Remove(CacheKeys.QuizzesWithQuestions(userId));
            return CreatedAtAction(nameof(Create), new { id = quiz.Id }, null);
        }

        // PUT: api/quiz/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] QuizDto quiz)
        {
            if (quiz == null) return BadRequest();
            await quizService.UpdateQuizAsync(id, quiz.Name ?? string.Empty, quiz.Questions ?? new List<QuestionDto>());
            (bool flowControl, ActionResult value) = GetCurrentUser(out Guid userId);
            if (!flowControl)
            {
               return value;
            }
            cache.Remove(CacheKeys.Quizzes(userId));
            cache.Remove(CacheKeys.QuizzesWithQuestions(userId));
            cache.Remove(CacheKeys.QuizWithQuestions(userId, id));
            return Ok();
        }

        // DELETE: api/quiz/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await quizService.DeleteQuizAsync(id);
            (bool flowControl, ActionResult value) = GetCurrentUser(out Guid userId);
            if (!flowControl)
            {
                return value;
            }
            cache.Remove(CacheKeys.Quizzes(userId));
            cache.Remove(CacheKeys.QuizzesWithQuestions(userId));
            cache.Remove(CacheKeys.QuizWithQuestions(userId, id));
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
