using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class UserFollowing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_AuthorId",
                table: "Decks");

            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Decks_DeckId",
                table: "Flashcards");

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
                name: "FK_Decks_Users_AuthorId",
                table: "Decks",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Decks_DeckId",
                table: "Flashcards",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_FollowerId",
                table: "Users",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_AuthorId",
                table: "Decks");

            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Decks_DeckId",
                table: "Flashcards");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_FollowerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FollowerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FollowerId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Users_AuthorId",
                table: "Decks",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Decks_DeckId",
                table: "Flashcards",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
