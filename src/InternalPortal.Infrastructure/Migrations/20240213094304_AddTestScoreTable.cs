using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InternalPortal.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class AddTestScoreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActual",
                schema: "test",
                table: "TestTopics",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActual",
                schema: "test",
                table: "TestQuestions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActual",
                schema: "test",
                table: "TestQuestionAnswers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TestsScore",
                schema: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    ProfileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestsScore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestsScore_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "dbo",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestsScore_ProfileId",
                schema: "test",
                table: "TestsScore",
                column: "ProfileId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestsScore",
                schema: "test");

            migrationBuilder.DropColumn(
                name: "IsActual",
                schema: "test",
                table: "TestTopics");

            migrationBuilder.DropColumn(
                name: "IsActual",
                schema: "test",
                table: "TestQuestions");

            migrationBuilder.DropColumn(
                name: "IsActual",
                schema: "test",
                table: "TestQuestionAnswers");
        }
    }
}
