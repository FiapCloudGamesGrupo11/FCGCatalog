using System.Text.Json;
using System.Text.Json.Serialization;

namespace FiapCloudGames.Infrastructure.Utils
{
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy/MM/dd HH:mm";

        public override DateTime Read (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString()!, Format, null);
        }

        public override void Write (Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
