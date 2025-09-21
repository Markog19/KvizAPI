using Microsoft.AspNetCore.Mvc;
using KvizAPI.Domain.Interfaces;
using KvizAPI.Application.DTO;
using Microsoft.AspNetCore.Authorization;

namespace KvizAPI.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController(IQuestionsService questionsService) : ControllerBase
    {
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAll()
        {
            var questions = await questionsService.GetAllAsync();
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetById(Guid id)
        {
            var question = await questionsService.GetByIdAsync(id);
            if (question == null) return NotFound();
            return Ok(question);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Create([FromBody] QuestionDto question)
        {
            if (question == null) return BadRequest();
            var created = await questionsService.CreateAsync(question);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] QuestionDto question)
        {
            if (question == null) return BadRequest();
            await questionsService.UpdateAsync(id, question);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await questionsService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search/{searchString}")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> Search(string searchString)
        {
            var results = await questionsService.SearchAsync(searchString);
            return Ok(results);
        }
    }
}
