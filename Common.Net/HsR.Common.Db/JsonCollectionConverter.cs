using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

public class JsonCollectionConverter : ValueConverter<ICollection<string>?, string?>
{
    public JsonCollectionConverter()
        : base(
            v => v == null ? "null" : JsonSerializer.Serialize(v, new JsonSerializerOptions()),  // Serialize null collection as null
            v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<ICollection<string>>(v, new JsonSerializerOptions()) // Deserialize empty or null JSON as null
        )
    { }
}
