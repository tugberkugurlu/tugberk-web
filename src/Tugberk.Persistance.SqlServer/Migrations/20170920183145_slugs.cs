using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Tugberk.Persistance.SqlServer.Migrations
{
    public partial class slugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostSlugEntity",
                columns: table => new
                {
                    Path = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    OwnedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSlugEntity", x => x.Path);
                    table.ForeignKey(
                        name: "FK_PostSlugEntity_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostSlugEntity_Posts_OwnedById",
                        column: x => x.OwnedById,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostSlugEntity_CreatedById",
                table: "PostSlugEntity",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostSlugEntity_OwnedById",
                table: "PostSlugEntity",
                column: "OwnedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostSlugEntity");
        }
    }
}
