using Microsoft.EntityFrameworkCore;
using KvizAPI.Domain.Entities;
using KvizAPI.Infrastructure.DBContexts;

namespace KvizAPI.Infrastructure.Import
{
    public static class SeedData
    {
        public static List<Question> Questions => new()
        {
            new Question { Text = "What is the capital of France?", Answer = "Paris" },
            new Question { Text = "What is 2 + 2?", Answer = "4", IsDeleted = false },
            new Question { Text = "What color do you get when you mix red and white?", Answer = "Pink" },
            new Question { Text = "Who wrote 'Hamlet'?", Answer = "William Shakespeare", IsDeleted = false },
            new Question { Text = "What is the boiling point of water at sea level in Celsius?", Answer = "100" },
            new Question { Text = "What planet is known as the Red Planet?", Answer = "Mars" },
            new Question { Text = "In computing, what does 'CPU' stand for?", Answer = "Central Processing Unit" },
            new Question { Text = "What is the largest mammal?", Answer = "Blue whale" },
            new Question { Text = "What language is primarily spoken in Brazil?", Answer = "Portuguese" },
            new Question { Text = "What year did the first man land on the moon?", Answer = "1969" },
            new Question { Text = "What is the capital of Japan?", Answer = "Tokyo" },
            new Question { Text = "Who painted the Mona Lisa?", Answer = "Leonardo da Vinci" },
            new Question { Text = "What is the chemical symbol for water?", Answer = "H2O" },
            new Question { Text = "Which element has the atomic number 1?", Answer = "Hydrogen" },
            new Question { Text = "How many continents are there on Earth?", Answer = "7" },
            new Question { Text = "What is the tallest mountain in the world?", Answer = "Mount Everest" },
            new Question { Text = "Who is the author of '1984'?", Answer = "George Orwell" },
            new Question { Text = "What currency is used in the United Kingdom?", Answer = "Pound sterling" },
            new Question { Text = "Which planet has the most moons?", Answer = "Saturn" },
            new Question { Text = "What is the square root of 64?", Answer = "8" },
            new Question { Text = "Which country hosted the 2016 Summer Olympics?", Answer = "Brazil" },
            new Question { Text = "What is the primary language spoken in Argentina?", Answer = "Spanish" },
            new Question { Text = "Who discovered penicillin?", Answer = "Alexander Fleming" },
            new Question { Text = "What is the largest ocean on Earth?", Answer = "Pacific Ocean" },
            new Question { Text = "What does HTTP stand for?", Answer = "HyperText Transfer Protocol" },
            new Question { Text = "What year did World War II end?", Answer = "1945" },
            new Question { Text = "What is the currency of Japan?", Answer = "Yen" },
            new Question { Text = "Which scientist developed the theory of relativity?", Answer = "Albert Einstein" },
            new Question { Text = "What is the smallest prime number?", Answer = "2" },
            new Question { Text = "What organ pumps blood through the body?", Answer = "Heart" },
            new Question { Text = "What is the chemical formula for table salt?", Answer = "NaCl" }
        };

        public static async Task EnsureSeededAsync(QuizDbContext db)
        {
            if (db == null) return;

            if (!await db.Questions.AnyAsync())
            {
                await db.Questions.AddRangeAsync(Questions);
                await db.SaveChangesAsync();
            }
        }
    }
}
