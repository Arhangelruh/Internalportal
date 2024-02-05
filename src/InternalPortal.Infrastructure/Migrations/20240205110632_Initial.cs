using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InternalPortal.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "test");

            migrationBuilder.CreateTable(
                name: "Profiles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false),
                    LastName = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false),
                    UserSid = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestTopics",
                schema: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TopicName = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTopics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestQuestions",
                schema: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionText = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TestTopicId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestQuestions_TestTopics_TestTopicId",
                        column: x => x.TestTopicId,
                        principalSchema: "test",
                        principalTable: "TestTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                schema: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TestTopicId = table.Column<int>(type: "integer", nullable: false),
                    ProfileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tests_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "dbo",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tests_TestTopics_TestTopicId",
                        column: x => x.TestTopicId,
                        principalSchema: "test",
                        principalTable: "TestTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestQuestionAnswers",
                schema: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnswerText = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Meaning = table.Column<bool>(type: "boolean", nullable: false),
                    TestQuestionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestQuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestQuestionAnswers_TestQuestions_TestQuestionId",
                        column: x => x.TestQuestionId,
                        principalSchema: "test",
                        principalTable: "TestQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestsAnswers",
                schema: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnswerId = table.Column<int>(type: "integer", nullable: false),
                    TestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestsAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestsAnswers_TestQuestionAnswers_AnswerId",
                        column: x => x.AnswerId,
                        principalSchema: "test",
                        principalTable: "TestQuestionAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestsAnswers_Tests_TestId",
                        column: x => x.TestId,
                        principalSchema: "test",
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestionAnswers_TestQuestionId",
                schema: "test",
                table: "TestQuestionAnswers",
                column: "TestQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestions_TestTopicId",
                schema: "test",
                table: "TestQuestions",
                column: "TestTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_ProfileId",
                schema: "test",
                table: "Tests",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TestTopicId",
                schema: "test",
                table: "Tests",
                column: "TestTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TestsAnswers_AnswerId",
                schema: "test",
                table: "TestsAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_TestsAnswers_TestId",
                schema: "test",
                table: "TestsAnswers",
                column: "TestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestsAnswers",
                schema: "test");

            migrationBuilder.DropTable(
                name: "TestQuestionAnswers",
                schema: "test");

            migrationBuilder.DropTable(
                name: "Tests",
                schema: "test");

            migrationBuilder.DropTable(
                name: "TestQuestions",
                schema: "test");

            migrationBuilder.DropTable(
                name: "Profiles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TestTopics",
                schema: "test");
        }
    }
}
