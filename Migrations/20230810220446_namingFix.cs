using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tsuHelp.Migrations
{
    public partial class namingFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Chats",
                newName: "SecondUserId");

            migrationBuilder.RenameColumn(
                name: "RecieverId",
                table: "Chats",
                newName: "FirstUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondUserId",
                table: "Chats",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "FirstUserId",
                table: "Chats",
                newName: "RecieverId");
        }
    }
}
