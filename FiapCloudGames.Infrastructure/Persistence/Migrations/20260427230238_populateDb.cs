using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapCloudGames.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class populateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlGames = @"
                INSERT INTO Game (Id, Name, Description, Category, Price ) VALUES
                ('11111111-1111-1111-1111-111111111111', 'The Legend of Zelda: Breath of the Wild', 'An open-world action-adventure game set in the kingdom of Hyrule.', 'Action-Adventure', 59.99),
                ('22222222-2222-2222-2222-222222222222', 'God of War', 'An action-adventure game following Kratos and his son Atreus on a journey through Norse mythology.', 'Action-Adventure', 49.99),
                ('33333333-3333-3333-3333-333333333333', 'Red Dead Redemption 2', 'An open-world action-adventure game set in the American Wild West.', 'Action-Adventure', 39.99),
                ('44444444-4444-4444-4444-444444444444', 'The Witcher 3: Wild Hunt', 'An open-world RPG following Geralt of Rivia as he hunts monsters and navigates political intrigue.', 'RPG', 29.99),
                ('55555555-5555-5555-5555-555555555555', 'Minecraft', 'A sandbox game that allows players to build and explore virtual worlds made of blocks.', 'Sandbox', 26.95);";
            migrationBuilder.Sql(sqlGames);

            var sqlUsers = @"
                    INSERT INTO [Users] (Id, Name, LastName, Email, Password, Role, Status, CreatedAt) VALUES
                    ('11111111-1111-1111-1111-111111111111', 'André', 'Silva', 'andre@email.com', '7f102f3cb49c8b89c23dd7617ffdcd45453f707549b3b5f15762ef9b9744e861', 1, 1, GETUTCDATE()), 
                    ('22222222-2222-2222-2222-222222222222', 'Carlos', 'Pereira', 'carlos@email.com', '84226cc341f6464f06aec7d3304c5243d6077dbd572dae8ba4f6459071c654cf', 2, 1, GETUTCDATE());";
            migrationBuilder.Sql(sqlUsers);


            var sqlUsersGames = @"
                INSERT INTO UsersGames (UserId, GameId, PurchaseDate, ValuePay) VALUES


                ('11111111-1111-1111-1111-111111111111', (SELECT Id FROM Game WHERE Name = 'Minecraft'), GETUTCDATE(), 26.95),
                ('11111111-1111-1111-1111-111111111111', (SELECT Id FROM Game WHERE Name = 'God of War'), GETUTCDATE(), 49.99),
                ('11111111-1111-1111-1111-111111111111', (SELECT Id FROM Game WHERE Name = 'Red Dead Redemption 2'), GETUTCDATE(), 39.99),

                ('22222222-2222-2222-2222-222222222222', (SELECT Id FROM Game WHERE Name = 'The Witcher 3: Wild Hunt'), GETUTCDATE(), 29.99),
                ('22222222-2222-2222-2222-222222222222', (SELECT Id FROM Game WHERE Name = 'Minecraft'), GETUTCDATE(), 26.95);
                ";

            migrationBuilder.Sql(sqlUsersGames);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
