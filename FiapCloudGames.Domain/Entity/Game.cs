using FiapCloudGames.Domain.Entity;

namespace FiapCloudGames.Domain.Entity
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }

        public IList<UsersGames> UsersGames { get; set; }
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

