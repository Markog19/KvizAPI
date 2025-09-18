using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KvizAPI.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionQuizzes",
                table: "QuestionQuizzes");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "QuestionQuizzes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionQuizzes",
                table: "QuestionQuizzes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionQuizzes_QuestionId",
                table: "QuestionQuizzes",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionQuizzes",
                table: "QuestionQuizzes");

            migrationBuilder.DropIndex(
                name: "IX_QuestionQuizzes_QuestionId",
                table: "QuestionQuizzes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "QuestionQuizzes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionQuizzes",
                table: "QuestionQuizzes",
                columns: new[] { "QuestionId", "QuizId" });
        }
    }
}
