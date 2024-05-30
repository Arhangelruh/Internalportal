using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalPortal.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class DeleteConnectionBetweenTestAndTopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_TestTopics_TestTopicId",
                schema: "test",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_TestTopicId",
                schema: "test",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TestTopicId",
                schema: "test",
                table: "Tests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestTopicId",
                schema: "test",
                table: "Tests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TestTopicId",
                schema: "test",
                table: "Tests",
                column: "TestTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_TestTopics_TestTopicId",
                schema: "test",
                table: "Tests",
                column: "TestTopicId",
                principalSchema: "test",
                principalTable: "TestTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
