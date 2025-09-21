using Plugins.DTOs;

namespace Plugins.Interface;
public interface IQuizPlugin
{
    string Name { get; }
    byte[] Execute(QuizDto quizDto);
}