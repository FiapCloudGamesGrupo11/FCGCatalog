using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FiapCloudGames.Infrastructure.Persistence.Mongo.Migrations
{
    // Aplica, uma única vez, cada IMongoMigration registrada que ainda não conste no histórico
    public class MongoMigrationRunner
    {
        private readonly MongoContext _context;
        private readonly IEnumerable<IMongoMigration> _migrations;
        private readonly ILogger<MongoMigrationRunner> _logger;

        public MongoMigrationRunner(MongoContext context, IEnumerable<IMongoMigration> migrations, ILogger<MongoMigrationRunner> logger)
        {
            _context = context;
            _migrations = migrations;
            _logger = logger;
        }

        public async Task MigrateAsync()
        {
            var history = _context.Database.GetCollection<MongoMigrationHistoryEntry>("__MongoMigrationsHistory");

            var appliedIds = (await history.Find(FilterDefinition<MongoMigrationHistoryEntry>.Empty).ToListAsync())
                .Select(entry => entry.Id)
                .ToHashSet();

            foreach (var migration in _migrations.OrderBy(m => m.Id))
            {
                if (appliedIds.Contains(migration.Id))
                    continue;

                await migration.UpAsync(_context.Database);

                await history.InsertOneAsync(new MongoMigrationHistoryEntry
                {
                    Id = migration.Id,
                    AppliedAt = DateTime.UtcNow
                });

                _logger.LogInformation("Mongo migration {MigrationId} applied.", migration.Id);
            }
        }
    }
}
