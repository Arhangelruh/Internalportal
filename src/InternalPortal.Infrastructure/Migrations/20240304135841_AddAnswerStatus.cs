using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalPortal.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class AddAnswerStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AnswerStatus",
                schema: "test",
                table: "TestsAnswers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerStatus",
                schema: "test",
                table: "TestsAnswers");
        }
    }
}
