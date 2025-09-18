using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvizAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToQuestionQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QuestionQuizzes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QuestionQuizzes");
        }
    }
}
