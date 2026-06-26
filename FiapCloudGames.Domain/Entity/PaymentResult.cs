using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiapCloudGames.Domain.Entity
{
    public class PaymentResult
    {
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;

        [JsonPropertyName("paymentId")]
        public string PaymentId { get; set; } = string.Empty;

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("gameId")]
        public Guid GameId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty; // "Approved" ou "Rejected"

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        [JsonPropertyName("processedAt")]
        public DateTime ProcessedAt { get; set; }
    }
}
