using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class FollowingUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_FollowerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FollowerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FollowerId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserFollowing",
                columns: table => new
                {
                    FollowerUserId = table.Column<int>(type: "int", nullable: false),
                    FollowingUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowing", x => new { x.FollowingUserId, x.FollowerUserId });
                    table.ForeignKey(
                        name: "FK_UserFollowing_Users_FollowerUserId",
                        column: x => x.FollowerUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFollowing_Users_FollowingUserId",
                        column: x => x.FollowingUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowing_FollowerUserId",
                table: "UserFollowing",
                column: "FollowerUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFollowing");

            migrationBuilder.AddColumn<int>(
                name: "FollowerId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FollowerId",
                table: "Users",
                column: "FollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_FollowerId",
                table: "Users",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
