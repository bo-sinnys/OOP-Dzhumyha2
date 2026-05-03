using System.Text.Json.Serialization;

namespace lab28v5.Models;

public class Song
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("artist")]
    public Artist Artist { get; set; } = new();

    [JsonPropertyName("genre")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Genre Genre { get; set; }

    [JsonPropertyName("duration_seconds")]
    public int DurationSeconds { get; set; }

    [JsonPropertyName("release_year")]
    public int ReleaseYear { get; set; }

    [JsonIgnore]
    public string DurationFormatted =>
        $"{DurationSeconds / 60}:{DurationSeconds % 60:D2}";

    public override string ToString() =>
        $"[Song #{Id}] \"{Title}\" — {Artist.Name} | {Genre} | {DurationFormatted} | {ReleaseYear}";
}
