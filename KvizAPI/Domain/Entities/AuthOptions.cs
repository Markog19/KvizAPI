namespace KvizAPI.Domain.Entities
{
    public class AuthOptions
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; } = "QuizAPI";

        public string Audience { get; set; } = "QuizClients";

    }
}