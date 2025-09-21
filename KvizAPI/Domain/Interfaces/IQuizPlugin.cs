using KvizAPI.Application.DTO;

public interface IQuizPlugin
{
    string Name { get; }
    byte[] Execute(QuizDto quizDto);
}