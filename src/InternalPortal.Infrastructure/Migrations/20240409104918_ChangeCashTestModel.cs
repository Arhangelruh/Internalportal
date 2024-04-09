using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalPortal.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCashTestModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestQuestions",
                schema: "test",
                table: "CashTests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WrongAnswers",
                schema: "test",
                table: "CashTests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestQuestions",
                schema: "test",
                table: "CashTests");

            migrationBuilder.DropColumn(
                name: "WrongAnswers",
                schema: "test",
                table: "CashTests");
        }
    }
}
