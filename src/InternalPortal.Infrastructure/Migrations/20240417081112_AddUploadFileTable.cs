using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InternalPortal.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class AddUploadFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_CashTests_CashTestId",
                schema: "test",
                table: "Tests");

            migrationBuilder.EnsureSchema(
                name: "file");

            migrationBuilder.CreateTable(
                name: "UploadFiles",
                schema: "file",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UntrastedName = table.Column<string>(type: "text", nullable: false),
                    TrustedName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFiles", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_CashTests_CashTestId",
                schema: "test",
                table: "Tests",
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
                name: "FK_Tests_CashTests_CashTestId",
                schema: "test",
                table: "Tests");

            migrationBuilder.DropTable(
                name: "UploadFiles",
                schema: "file");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_CashTests_CashTestId",
                schema: "test",
                table: "Tests",
                column: "CashTestId",
                principalSchema: "test",
                principalTable: "CashTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
