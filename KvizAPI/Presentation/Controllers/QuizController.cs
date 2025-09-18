using Microsoft.AspNetCore.Mvc;
using KvizAPI.Application.DTO;

namespace KvizAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<QuizDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        // GET: api/quiz/{id}
        [HttpGet("{id}")]
        public ActionResult<QuizDto> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        // POST: api/quiz
        [HttpPost]
        public ActionResult<QuizDto> Create([FromBody] QuizDto quiz)
        {
            throw new NotImplementedException();
        }

        // PUT: api/quiz/{id}
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] QuizDto quiz)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/quiz/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
