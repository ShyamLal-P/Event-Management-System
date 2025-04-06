using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace EventManagementSystem.Helpers
{
    public class TimeOnlyJsonConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var timeString = reader.GetString();

            // Handle HH:mm or HH:mm:ss
            if (TimeSpan.TryParseExact(timeString, new[] { @"hh\:mm", @"hh\:mm\:ss" }, null, out var time))
            {
                return time;
            }

            throw new JsonException("Invalid time format. Expected HH:mm or HH:mm:ss.");
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(@"hh\:mm"));
        }
    }
}
