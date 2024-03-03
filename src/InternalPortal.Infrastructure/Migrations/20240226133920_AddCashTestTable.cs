using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InternalPortal.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class AddCashTestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CashTestId",
                schema: "test",
                table: "TestTopics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CashTests",
                schema: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestName = table.Column<string>(type: "character varying(63)", maxLength: 63, nullable: false),
                    IsActual = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashTests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestTopics_CashTestId",
                schema: "test",
                table: "TestTopics",
                column: "CashTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestTopics_CashTests_CashTestId",
                schema: "test",
                table: "TestTopics",
                column: "CashTestId",
                principalSchema: "test",
                principalTable: "CashTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestTopics_CashTests_CashTestId",
                schema: "test",
                table: "TestTopics");

            migrationBuilder.DropTable(
                name: "CashTests",
                schema: "test");

            migrationBuilder.DropIndex(
                name: "IX_TestTopics_CashTestId",
                schema: "test",
                table: "TestTopics");

            migrationBuilder.DropColumn(
                name: "CashTestId",
                schema: "test",
                table: "TestTopics");
        }
    }
}
