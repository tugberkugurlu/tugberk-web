using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tugberk.Persistance.SqlServer.Migrations
{
    public partial class FlattenApprovalStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostApprovalStatusActionEntity");
            
            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "Posts",
                nullable: false,
                defaultValue: 0);
            
            // see https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/operations 
            // All the records we have so far are approved. So, set them to approved and move on!
            migrationBuilder.Sql("UPDATE Posts SET ApprovalStatus = 1;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "PostApprovalStatusActionEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PostId = table.Column<Guid>(nullable: false),
                    RecordedById = table.Column<string>(nullable: false),
                    RecordedOnUtc = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostApprovalStatusActionEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostApprovalStatusActionEntity_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostApprovalStatusActionEntity_AspNetUsers_RecordedById",
                        column: x => x.RecordedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostApprovalStatusActionEntity_PostId",
                table: "PostApprovalStatusActionEntity",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostApprovalStatusActionEntity_RecordedById",
                table: "PostApprovalStatusActionEntity",
                column: "RecordedById");
        }
    }
}
