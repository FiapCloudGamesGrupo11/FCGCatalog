using System;
using System.Text.Json.Serialization;
using FiapCloudGames.Infrastructure.Utils;

namespace FiapCloudGames.Application.DTOs.OnSale.Request
{
    public class OnSaleRequest
    {
        public Guid GameId { get; set; }
        public decimal DiscountPercentage { get; set; }

        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime StartDate { get; set; }

        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime EndDate { get; set; }
    }
}
