using Microsoft.EntityFrameworkCore;
using KvizAPI.Domain.Entities;

namespace KvizAPI.Infrastructure.DBContexts
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
        {
        }

        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionQuiz> QuestionQuizzes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite key for join table
            modelBuilder.Entity<QuestionQuiz>()
                .HasKey(qt => new { qt.QuestionId, qt.QuizId });

            // Configure relationships
            modelBuilder.Entity<QuestionQuiz>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionQuizzes)
                .HasForeignKey(qt => qt.QuestionId);

            modelBuilder.Entity<QuestionQuiz>()
                .HasOne(qt => qt.Quiz)
                .WithMany(qz => qz.QuestionQuizzes)
                .HasForeignKey(qt => qt.QuizId);

            // Add index on Quiz.Name for faster lookups
            modelBuilder.Entity<Quiz>()
                .HasIndex(q => q.Name)
                .HasDatabaseName("IX_Quizzes_Name");
        }
    }
}
