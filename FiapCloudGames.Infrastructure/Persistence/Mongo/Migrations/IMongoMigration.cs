using MongoDB.Driver;

namespace FiapCloudGames.Infrastructure.Persistence.Mongo.Migrations
{
    // Equivalente a uma EF Core Migration, porém para coleções do MongoDB
    public interface IMongoMigration
    {
        // Segue o padrão "yyyyMMddHHmmss_Nome", igual ao nome gerado pelo EF Core
        string Id { get; }

        Task UpAsync(IMongoDatabase database);
    }
}
