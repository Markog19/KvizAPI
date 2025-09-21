using KvizAPI.Application.DTO;
using KvizAPI.Application.Services;
using KvizAPI.Domain.Interfaces;
using KvizAPI.Infrastructure.DBContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plugins.Interface;
using System.Composition.Hosting;
using System.Reflection;

namespace KvizAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly IEnumerable<IQuizPlugin> Plugins;
        private readonly QuizDbContext _dbContext;

        public IQuizService _quizService { get; }
        //C:\\Users\\User\\source\\repos\\KvizAPI\\KvizAPI\\bin\\Debug\\net8.0\\
        public ExportController(QuizDbContext dbContext, IQuizService quizService)
        {
            var root = AppContext.BaseDirectory;
            var pluginPath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Plugins", "bin", "Debug", "net8.0")
            );
            var pluginAssemblies = Directory.GetFiles(pluginPath, "*.dll")
                .Select(Assembly.LoadFrom);
            var config = new ContainerConfiguration().WithAssemblies(pluginAssemblies);
            using var container = config.CreateContainer();
            Plugins = container.GetExports<IQuizPlugin>();
            _dbContext = dbContext;
            _quizService = quizService;
        }
        [HttpGet]
        public IActionResult GetPlugins()
        {
            var pluginNames = Plugins.Select(p => p.Name).ToList();
            return Ok(pluginNames);
        }
        [Authorize]
        [HttpGet("{quizId}/{pluginName}")]
        public async Task<IActionResult> ExecutePluginAsync(string pluginName, Guid quizId)
        {
            var plugin = Plugins.FirstOrDefault(p => p.Name.Equals(pluginName, StringComparison.OrdinalIgnoreCase));
            if (plugin == null)
            {
                return NotFound($"Plugin '{pluginName}' not found.");
            }

            (bool flowControl, ActionResult value) = GetCurrentUser(out Guid userId);
            if (!flowControl)
            {
                return value;
            }
            var quizData = await _quizService.GetQuizzWithQuestionsAsync(userId, quizId);
            if (quizData == null)
            {
                return NotFound("Quiz not found.");
            }
            var exportedBytes = plugin.Execute(quizData);
            var fileName = $"Quiz_{SanitizeFileName(quizData.Name)}.csv";
            return File(exportedBytes, "text/csv", fileName);
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

        private static string SanitizeFileName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "Unnamed";
            }
            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }
    }
}
