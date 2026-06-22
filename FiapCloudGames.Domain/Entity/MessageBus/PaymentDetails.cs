using System.Text.Json.Serialization;

namespace FiapCloudGames.Domain.Entity.MessageBus
{
    public record PaymentDetails
    {
        public PaymentDetails() { }
        public PaymentDetails(string paymentMethod, string? cardNumber, string? cvv, string? expirationDate)
        {
            PaymentMethod = paymentMethod;
            CardNumber = cardNumber;
            Cvv = cvv;
            ExpirationDate = expirationDate;
        }

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; } = string.Empty; // "CREDIT_CARD", "PIX", "BOLETO"

        [JsonPropertyName("cardNumber")]
        public string? CardNumber { get; set; }

        [JsonPropertyName("cvv")]
        public string? Cvv { get; set; }

        [JsonPropertyName("expirationDate")]
        public string? ExpirationDate { get; set; }

    }
}
