using MongoDB.Bson.Serialization.Attributes;

namespace FiapCloudGames.Domain.Entity
{
    public class Game
    {
        [BsonId]
        public Guid Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("category")]
        public string Category { get; set; }
        [BsonElement("price")]
        public decimal Price { get; set; }

        public ICollection<OnSale> OnSales { get; set; } = new List<OnSale>();

        public Game () { }
        public Game(string name, decimal price, string description, string category)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Description = description;
            Category = category;
        }

    public static Game Create(string name, decimal price, string description, string category)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome não pode ser vazio ou nulo.", nameof(name));

            if(price <= 0)
                throw new ArgumentException("Preço deve ser maior que zero.", nameof(price));

            if(string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição não pode ser vazia ou nula.", nameof(description));

            if(string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Categoria não pode ser vazia ou nula.", nameof(category));

            return new Game(name, price, description, category);
        }
    }
}

