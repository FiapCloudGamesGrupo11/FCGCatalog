using MongoDB.Bson.Serialization.Attributes;

namespace FiapCloudGames.Infrastructure.Persistence.Mongo.Migrations
{
    // Equivalente à tabela __EFMigrationsHistory do EF Core, mas para o MongoDB
    public class MongoMigrationHistoryEntry
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;

        public DateTime AppliedAt { get; set; }
    }
}
