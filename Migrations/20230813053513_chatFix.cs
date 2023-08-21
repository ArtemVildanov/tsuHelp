using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tsuHelp.Migrations
{
    public partial class chatFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Chats_ChatId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_ChatId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Chats");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "Chats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ChatId",
                table: "Chats",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Chats_ChatId",
                table: "Chats",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id");
        }
    }
}
