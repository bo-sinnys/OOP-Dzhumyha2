using System.Text.Json;
using System.Text.Encodings.Web;
using lab28v5.Models;

namespace lab28v5.Repositories;

public class SongRepository
{
    private readonly List<Song> _songs = new();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    // ── CRUD ──────────────────────────────────────────────────────────────────

    /// <summary>Додає пісню до репозиторію.</summary>
    public void Add(Song song)
    {
        ArgumentNullException.ThrowIfNull(song);
        _songs.Add(song);
        Console.WriteLine($"  + Додано: {song}");
    }

    /// <summary>Повертає всі пісні.</summary>
    public IReadOnlyList<Song> GetAll() => _songs.AsReadOnly();

    /// <summary>Повертає пісню за Id або null.</summary>
    public Song? GetById(int id) =>
        _songs.FirstOrDefault(s => s.Id == id);

    // ── JSON I/O ──────────────────────────────────────────────────────────────

    /// <summary>Асинхронно зберігає всі пісні у JSON-файл.</summary>
    public async Task SaveToFileAsync(string filename)
    {
        try
        {
            string dir = Path.GetDirectoryName(filename) ?? string.Empty;
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            await using FileStream stream = File.Create(filename);
            await JsonSerializer.SerializeAsync(stream, _songs, JsonOptions);

            Console.WriteLine($"  Збережено {_songs.Count} пісень → {filename}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"  [Помилка запису] {ex.Message}");
            throw;
        }
    }

    /// <summary>Асинхронно завантажує пісні з JSON-файлу.</summary>
    public async Task LoadFromFileAsync(string filename)
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException($"Файл не знайдено: {filename}");

        try
        {
            await using FileStream stream = File.OpenRead(filename);
            var loaded = await JsonSerializer.DeserializeAsync<List<Song>>(stream, JsonOptions);

            if (loaded is null || loaded.Count == 0)
            {
                Console.WriteLine("  Файл порожній або містить некоректні дані.");
                return;
            }

            _songs.Clear();
            _songs.AddRange(loaded);

            Console.WriteLine($"  Завантажено {_songs.Count} пісень ← {filename}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"  [Помилка JSON] {ex.Message}");
            throw;
        }
    }
}
