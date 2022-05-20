using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class reviewrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RevieweeId",
                table: "Reviews",
                column: "RevieweeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_RevieweeId",
                table: "Reviews",
                column: "RevieweeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_RevieweeId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_RevieweeId",
                table: "Reviews");
        }
    }
}
