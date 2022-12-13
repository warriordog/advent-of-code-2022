using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdventOfCode.Day13;

/// <summary>
/// Serialization support for polymorphic <see cref="PacketValue"/>s.
/// </summary>
public class PacketValueConverter : JsonConverter<PacketValue>
{
    public override PacketValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Why is this suggestion set at warning level by default??
        // Surely its a common pattern to handle the acceptable values and throw on the rest?
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        return reader.TokenType switch
        {
            // Int value
            JsonTokenType.Number => ReadIntValue(ref reader),
            
            // Array value
            JsonTokenType.StartArray => ReadArrayValue(ref reader, options),
            
            // Invalid value
            _ => throw new JsonException($"PacketValue can only be a number or an array. Got unsupported token type {reader.TokenType}")
        };
    }
    
    private static PacketValue ReadIntValue(ref Utf8JsonReader reader)
    {
        var intValue = reader.GetInt32();
        return new PacketValue(intValue);
    }
    
    private static PacketValue ReadArrayValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var listValue = JsonSerializer.Deserialize<PacketValue[]>(ref reader, options);
        if (listValue == null)
        {
            throw new JsonException("Failed to deserialize PacketValue - list deserialized to a null object");
        }
        
        return new PacketValue(listValue);
    }

    public override void Write(Utf8JsonWriter writer, PacketValue value, JsonSerializerOptions options)
    {
        if (value.IsArray)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
        else
        {
            writer.WriteNumberValue(value.IntValue.Value);
        }
    }
}