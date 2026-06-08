namespace FiapCloudGames.Application.DTOs.OnSale.Response
{
    public class OnSaleResponse
    {
        public Guid Id { get; set; }
        public string GameName { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountedPrice { get; set; }
    }
}
