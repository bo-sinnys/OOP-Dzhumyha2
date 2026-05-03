using lab28v5.Models;
using lab28v5.Repositories;

// ── Крок 1: Заповнення репозиторію ───────────────────────────────────────────

Console.WriteLine("=== Лабораторна робота №28 — Серіалізація JSON ===\n");

var repo = new SongRepository();

var artists = new[]
{
    new Artist { Id = 1, Name = "The Beatles",    Country = "Велика Британія", BirthYear = 1960 },
    new Artist { Id = 2, Name = "Billie Eilish",  Country = "США",             BirthYear = 2001 },
    new Artist { Id = 3, Name = "Imagine Dragons", Country = "США",            BirthYear = 2008 },
    new Artist { Id = 4, Name = "Adele",           Country = "Велика Британія", BirthYear = 1988 },
};

Console.WriteLine("[ Додавання пісень ]");
repo.Add(new Song
{
    Id = 1, Title = "Let It Be", Artist = artists[0],
    Genre = Genre.Rock, DurationSeconds = 243, ReleaseYear = 1970
});
repo.Add(new Song
{
    Id = 2, Title = "bad guy", Artist = artists[1],
    Genre = Genre.Pop, DurationSeconds = 194, ReleaseYear = 2019
});
repo.Add(new Song
{
    Id = 3, Title = "Believer", Artist = artists[2],
    Genre = Genre.Rock, DurationSeconds = 204, ReleaseYear = 2017
});
repo.Add(new Song
{
    Id = 4, Title = "Rolling in the Deep", Artist = artists[3],
    Genre = Genre.Pop, DurationSeconds = 228, ReleaseYear = 2010
});
repo.Add(new Song
{
    Id = 5, Title = "Yesterday", Artist = artists[0],
    Genre = Genre.Rock, DurationSeconds = 125, ReleaseYear = 1965
});

// ── Крок 2: Збереження у файл ─────────────────────────────────────────────────

const string FilePath = "data/songs.json";

Console.WriteLine("\n[ Збереження у JSON-файл ]");
await repo.SaveToFileAsync(FilePath);

// ── Крок 3: Завантаження з файлу ─────────────────────────────────────────────

Console.WriteLine("\n[ Завантаження з JSON-файлу ]");
var loadedRepo = new SongRepository();
await loadedRepo.LoadFromFileAsync(FilePath);

// ── Крок 4: Вивід результатів ─────────────────────────────────────────────────

Console.WriteLine("\n[ Усі пісні після завантаження ]");
Console.WriteLine(new string('─', 70));
foreach (var song in loadedRepo.GetAll())
    Console.WriteLine($"  {song}");

Console.WriteLine("\n[ Пошук за Id = 3 ]");
Console.WriteLine(new string('─', 70));
var found = loadedRepo.GetById(3);
Console.WriteLine(found is not null ? $"  Знайдено: {found}" : "  Не знайдено.");

// ── Крок 5: Вміст JSON-файлу ──────────────────────────────────────────────────

Console.WriteLine("\n[ Вміст файлу songs.json ]");
Console.WriteLine(new string('─', 70));
Console.WriteLine(await File.ReadAllTextAsync(FilePath));
