using System.Text.Json;
using System.Text.Json.Serialization;

namespace HaidaiTech.Enumerator.Conversors
{
    public class EnumerationConverter<T> : JsonConverter<T> where T : Enumeration
    {
        /// <summary>
        /// Reads and converts the JSON to the specified type.
        /// </summary>
        /// <param name="reader">The reader to read the JSON from.</param>
        /// <param name="typeToConvert">The type to convert to.</param>
        /// <param name="options">Options to control the conversion behavior.</param>
        /// <returns>The converted value of type T.</returns>
        /// <exception cref="JsonException">Thrown when the JSON value cannot be converted to the specified type.</exception>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString() ?? string.Empty;
                return Enumeration.FromName<T>(value);
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                int value = reader.GetInt32();
                return Enumeration.FromId<T>(value);
            }

            throw new JsonException($"Unable to convert value to {typeof(T).Name}.");
        }

        /// <summary>
        /// Writes the specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write the JSON to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">Options to control the conversion behavior.</param>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }
}