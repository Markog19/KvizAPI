using Microsoft.EntityFrameworkCore;
using KvizAPI.Domain.Entities;

namespace KvizAPI.Infrastructure.DBContexts
{
    public class QuizDbContext(DbContextOptions<QuizDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
    {
        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionQuiz> QuestionQuizzes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuestionQuiz>()
                .HasKey(qt => qt.Id);

            modelBuilder.Entity<QuestionQuiz>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionQuizzes)
                .HasForeignKey(qt => qt.QuestionId);

            modelBuilder.Entity<QuestionQuiz>()
                .HasOne(qt => qt.Quiz)
                .WithMany(qz => qz.QuestionQuizzes)
                .HasForeignKey(qt => qt.QuizId);
        }
    }
}
