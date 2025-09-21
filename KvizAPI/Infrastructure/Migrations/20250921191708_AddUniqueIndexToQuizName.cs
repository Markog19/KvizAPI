using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvizAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToQuizName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizzes_Name",
                table: "Quizzes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Quizzes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_Name",
                table: "Quizzes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizzes_Name",
                table: "Quizzes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Quizzes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_Name",
                table: "Quizzes",
                column: "Name");
        }
    }
}
