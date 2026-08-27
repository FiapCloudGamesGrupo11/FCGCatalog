using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapCloudGames.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUsersGamesRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersGames_Game_GameId",
                table: "UsersGames");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersGames_Users_UserId",
                table: "UsersGames");

            migrationBuilder.DropIndex(
                name: "IX_UsersGames_GameId",
                table: "UsersGames");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UsersGames_GameId",
                table: "UsersGames",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGames_Game_GameId",
                table: "UsersGames",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGames_Users_UserId",
                table: "UsersGames",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
