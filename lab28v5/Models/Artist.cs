using System.Text.Json.Serialization;

namespace lab28v5.Models;

public class Artist
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("birth_year")]
    public int BirthYear { get; set; }

    public override string ToString() =>
        $"[Artist #{Id}] {Name} ({Country}, {BirthYear})";
}
