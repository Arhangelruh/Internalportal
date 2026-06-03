using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InternalPortal.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class AddReadingReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReadingReview",
                schema: "file",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReviewTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProfileId = table.Column<int>(type: "integer", nullable: false),
                    FileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingReview_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "dbo",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReadingReview_UploadFiles_FileId",
                        column: x => x.FileId,
                        principalSchema: "file",
                        principalTable: "UploadFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingReview_FileId",
                schema: "file",
                table: "ReadingReview",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingReview_ProfileId",
                schema: "file",
                table: "ReadingReview",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadingReview",
                schema: "file");
        }
    }
}
