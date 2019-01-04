using Microsoft.EntityFrameworkCore.Migrations;

namespace Tugberk.Persistance.SqlServer.Migrations
{
    public partial class required_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostApprovalStatusActionEntity_Posts_PostId",
                table: "PostApprovalStatusActionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostApprovalStatusActionEntity_AspNetUsers_RecordedById",
                table: "PostApprovalStatusActionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_CreatedById",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostSlugEntity_AspNetUsers_CreatedById",
                table: "PostSlugEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostSlugEntity_Posts_OwnedById",
                table: "PostSlugEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTagEntity_Posts_PostId",
                table: "PostTagEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTagEntity_Tags_TagName",
                table: "PostTagEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetUsers_CreatedById",
                table: "Tags");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostApprovalStatusActionEntity_Posts_PostId",
                table: "PostApprovalStatusActionEntity",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostApprovalStatusActionEntity_AspNetUsers_RecordedById",
                table: "PostApprovalStatusActionEntity",
                column: "RecordedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_CreatedById",
                table: "Posts",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostSlugEntity_AspNetUsers_CreatedById",
                table: "PostSlugEntity",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostSlugEntity_Posts_OwnedById",
                table: "PostSlugEntity",
                column: "OwnedById",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTagEntity_Posts_PostId",
                table: "PostTagEntity",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTagEntity_Tags_TagName",
                table: "PostTagEntity",
                column: "TagName",
                principalTable: "Tags",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetUsers_CreatedById",
                table: "Tags",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostApprovalStatusActionEntity_Posts_PostId",
                table: "PostApprovalStatusActionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostApprovalStatusActionEntity_AspNetUsers_RecordedById",
                table: "PostApprovalStatusActionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_CreatedById",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostSlugEntity_AspNetUsers_CreatedById",
                table: "PostSlugEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostSlugEntity_Posts_OwnedById",
                table: "PostSlugEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTagEntity_Posts_PostId",
                table: "PostTagEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTagEntity_Tags_TagName",
                table: "PostTagEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetUsers_CreatedById",
                table: "Tags");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_PostApprovalStatusActionEntity_Posts_PostId",
                table: "PostApprovalStatusActionEntity",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostApprovalStatusActionEntity_AspNetUsers_RecordedById",
                table: "PostApprovalStatusActionEntity",
                column: "RecordedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_CreatedById",
                table: "Posts",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostSlugEntity_AspNetUsers_CreatedById",
                table: "PostSlugEntity",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostSlugEntity_Posts_OwnedById",
                table: "PostSlugEntity",
                column: "OwnedById",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTagEntity_Posts_PostId",
                table: "PostTagEntity",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTagEntity_Tags_TagName",
                table: "PostTagEntity",
                column: "TagName",
                principalTable: "Tags",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetUsers_CreatedById",
                table: "Tags",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
