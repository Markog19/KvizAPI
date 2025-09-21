using Plugins.DTOs;
using Plugins.Interface;
using System.Composition;
using System.Text;

[Export(typeof(IQuizPlugin))]
public class ExportToCSVPlugin : IQuizPlugin
{
    public string Name => "Export To CSV Plugin";

    // Change: Return CSV as byte[]
    public byte[] Execute(QuizDto quizDto)
    {
        if (quizDto == null || quizDto.Questions == null || quizDto.Questions.Count == 0)
        {
            return Array.Empty<byte>();
        }

        var sb = new StringBuilder();

        sb.AppendLine("Question");

        foreach (var q in quizDto.Questions)
        {
            var question = EscapeForCsv(q.Text);
            sb.AppendLine($"{question}");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static string EscapeForCsv(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "";
        }
        var needsQuotes = value.Contains(',') || value.Contains('"') || value.Contains('\n');
        var escaped = value.Replace("\"", "\"\"");
        return needsQuotes ? $"\"{escaped}\"" : escaped;
    }

    
}