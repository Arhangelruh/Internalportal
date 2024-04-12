using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalPortal.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionTestToConnectionTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CashTestId",
                schema: "test",
                table: "Tests",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_CashTestId",
                schema: "test",
                table: "Tests",
                column: "CashTestId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_CashTests_CashTestId",
                schema: "test",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_CashTestId",
                schema: "test",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "CashTestId",
                schema: "test",
                table: "Tests");
        }
    }
}
