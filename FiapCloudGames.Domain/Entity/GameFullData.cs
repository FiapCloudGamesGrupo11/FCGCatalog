using MongoDB.Bson.Serialization.Attributes;

namespace FiapCloudGames.Domain.Entity
{
    public class GameFullData
    {
        public GameFullData()
        {
            Name = string.Empty;
            Description = string.Empty;
            Category = string.Empty;
            Developer = string.Empty;
            Publisher = string.Empty;
            Genres = new List<string>();
            Platforms = new List<string>();
            Rating = string.Empty;
            Tags = new List<string>();
        }

        public static GameFullData TransferData(
            string name,
            string description,
            string category,
            string developer,
            string publisher,
            IEnumerable<string> genres,
            IEnumerable<string> platforms,
            decimal price,
            DateTime releaseDate,
            string rating,
            IEnumerable<string> tags)
        {
            return new GameFullData
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Category = category,
                Developer = developer,
                Publisher = publisher,
                Genres = genres.ToList(),
                Platforms = platforms.ToList(),
                Price = price,
                ReleaseDate = releaseDate,
                Rating = rating,
                Tags = tags.ToList()
            };
        }

        [BsonId]
        public Guid Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("category")]
        public string Category { get; set; }
        [BsonElement("developer")]
        public string Developer { get; set; }
        [BsonElement("publisher")]
        public string Publisher { get; set; }
        [BsonElement("genres")]
        public List<string> Genres { get; set; }
        [BsonElement("platform")]
        public List<string> Platforms { get; set; }
        [BsonElement("price")]
        public decimal Price { get; set; }
        [BsonElement("releaseDate")]
        public DateTime ReleaseDate { get; set; }
        [BsonElement("rating")]
        public string Rating { get; set; }
        [BsonElement("tags")]
        public List<string> Tags { get; set; }





        

    }
}