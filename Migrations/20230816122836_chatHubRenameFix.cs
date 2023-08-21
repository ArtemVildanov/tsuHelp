using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tsuHelp.Migrations
{
    public partial class chatHubRenameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_AspNetUsers_UserId",
                table: "UserConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserConnections",
                table: "UserConnections");

            migrationBuilder.RenameTable(
                name: "UserConnections",
                newName: "UserChatHubConnections");

            migrationBuilder.RenameIndex(
                name: "IX_UserConnections_UserId",
                table: "UserChatHubConnections",
                newName: "IX_UserChatHubConnections_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChatHubConnections",
                table: "UserChatHubConnections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChatHubConnections_AspNetUsers_UserId",
                table: "UserChatHubConnections",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChatHubConnections_AspNetUsers_UserId",
                table: "UserChatHubConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChatHubConnections",
                table: "UserChatHubConnections");

            migrationBuilder.RenameTable(
                name: "UserChatHubConnections",
                newName: "UserConnections");

            migrationBuilder.RenameIndex(
                name: "IX_UserChatHubConnections_UserId",
                table: "UserConnections",
                newName: "IX_UserConnections_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserConnections",
                table: "UserConnections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_AspNetUsers_UserId",
                table: "UserConnections",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
