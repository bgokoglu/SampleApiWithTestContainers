using System.Text.Json.Serialization;

namespace SampleApiWithTestContainers.Products;

public sealed class ProductResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("createdDtTm")]
    public DateTimeOffset CreatedDtTm { get; set; }
}