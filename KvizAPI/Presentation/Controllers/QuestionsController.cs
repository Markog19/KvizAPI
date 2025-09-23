using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KvizAPI.Domain.Interfaces;
using KvizAPI.Application.DTO;
using Microsoft.Extensions.Caching.Memory;
using KvizAPI.Application.Common;
using Microsoft.Extensions.Options;
using Plugins.DTOs;

namespace KvizAPI.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController(IQuestionsService questionsService, IMemoryCache cache, IOptions<CacheSettings> cacheSettings) : ControllerBase
    {
     

        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAll()
        {
            const string cacheKey = "questions_all";
            if (!cache.TryGetValue(cacheKey, out IEnumerable<QuestionDto> questions))
            {
                questions = await questionsService.GetAllAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(cacheSettings.Value.QuizzesExpirationMinutes));
                cache.Set(cacheKey, questions, cacheEntryOptions);
            }
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetById(Guid id)
        {
            var cacheKey = $"question_{id}";
            if (!cache.TryGetValue(cacheKey, out QuestionDto? question))
            {
                question = await questionsService.GetByIdAsync(id);
                if (question == null) return NotFound();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(cacheSettings.Value.QuizWithQuestionsExpirationMinutes));
                cache.Set(cacheKey, question, cacheEntryOptions);
            }
            return Ok(question);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Create([FromBody] QuestionDto question)
        {
            if (question == null) return BadRequest();
            var created = await questionsService.CreateAsync(question);
            cache.Remove("questions_all");
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] QuestionDto question)
        {
            if (question == null) return BadRequest();
            await questionsService.UpdateAsync(id, question);
            cache.Remove("questions_all");
            cache.Remove($"question_{id}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await questionsService.DeleteAsync(id);
            cache.Remove("questions_all");
            cache.Remove($"question_{id}");
            return NoContent();
        }

        [HttpGet("search/{searchString}")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> Search(string searchString)
        {
            var cacheKey = $"questions_search_{searchString}";
            if (!cache.TryGetValue(cacheKey, out IEnumerable<QuestionDto> results))
            {
                results = await questionsService.SearchAsync(searchString);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(cacheSettings.Value.QuizzesWithQuestionsExpirationMinutes));
                cache.Set(cacheKey, results, cacheEntryOptions);
            }
            return Ok(results);
        }
    }
}
