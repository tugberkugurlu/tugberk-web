using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Tugberk.Persistance.SqlServer.Migrations
{
    public partial class approval_actions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostApprovalStatusActionEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecordedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostApprovalStatusActionEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostApprovalStatusActionEntity_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostApprovalStatusActionEntity_AspNetUsers_RecordedById",
                        column: x => x.RecordedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostApprovalStatusActionEntity");
        }
    }
}
