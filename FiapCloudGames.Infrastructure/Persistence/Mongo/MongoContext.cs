using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;

namespace FiapCloudGames.Infrastructure.Persistence.Mongo
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        private static readonly object SerializerLock = new();
        private static bool _guidSerializerRegistered;

        public MongoContext(IOptions<MongoSettings> options)
        {
            lock (SerializerLock)
            {
                if (!_guidSerializerRegistered)
                {
                    BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                    _guidSerializerRegistered = true;
                }
            }

            var settings = options.Value;

            var client = new MongoClient(settings.ConnectionString);

            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoDatabase Database => _database;
    }
}