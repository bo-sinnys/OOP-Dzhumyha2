# Лабораторна робота №28 
**Тема:** Серіалізація об'єктів у JSON
---

## Мета

Навчитися серіалізувати та десеріалізувати складні об'єкти у форматі JSON за допомогою `System.Text.Json`, зберігати дані у файли та завантажувати їх асинхронно.

---

## Структура проєкту

```
lab28v5/
├── Models/
│   ├── Genre.cs            ← перелік музичних жанрів
│   ├── Artist.cs           ← клас виконавця
│   └── Song.cs             ← клас пісні (вкладений Artist)
├── Repositories/
│   └── SongRepository.cs   ← репозиторій з JSON-серіалізацією
├── data/
│   └── songs.json          ← файл з даними (генерується автоматично)
└── Program.cs              ← демонстрація роботи
```

---

## Класи предметної області

### `Genre` (enum)

Перелік музичних жанрів:

```csharp
public enum Genre
{
    Pop, Rock, Jazz, Classical,
    HipHop, Electronic, RnB, Country
}
```

### `Artist`

Клас, що описує музичного виконавця:

| Властивість | Тип | JSON-ім'я | Опис |
|---|---|---|---|
| `Id` | `int` | `id` | Унікальний ідентифікатор |
| `Name` | `string` | `name` | Ім'я виконавця |
| `Country` | `string` | `country` | Країна походження |
| `BirthYear` | `int` | `birth_year` | Рік заснування/народження |

### `Song`

Клас, що описує пісню з вкладеним об'єктом `Artist`:

| Властивість | Тип | JSON-ім'я | Опис |
|---|---|---|---|
| `Id` | `int` | `id` | Унікальний ідентифікатор |
| `Title` | `string` | `title` | Назва пісні |
| `Artist` | `Artist` | `artist` | Виконавець (вкладений об'єкт) |
| `Genre` | `Genre` | `genre` | Жанр (рядком: `"Rock"`) |
| `DurationSeconds` | `int` | `duration_seconds` | Тривалість у секундах |
| `ReleaseYear` | `int` | `release_year` | Рік випуску |
| `DurationFormatted` | `string` | — | Формат `M:SS`, **ігнорується** `[JsonIgnore]` |

---

## Репозиторій `SongRepository`

### Методи CRUD

| Метод | Опис |
|---|---|
| `Add(Song)` | Додає пісню до колекції |
| `GetAll()` | Повертає всі пісні (read-only) |
| `GetById(int id)` | Повертає пісню за Id або `null` |

### Методи серіалізації

```csharp
// Асинхронне збереження у JSON-файл
public async Task SaveToFileAsync(string filename)
{
    await using FileStream stream = File.Create(filename);
    await JsonSerializer.SerializeAsync(stream, _songs, JsonOptions);
}

// Асинхронне завантаження з JSON-файлу
public async Task LoadFromFileAsync(string filename)
{
    await using FileStream stream = File.OpenRead(filename);
    var loaded = await JsonSerializer.DeserializeAsync<List<Song>>(stream, JsonOptions);
    _songs.AddRange(loaded!);
}
```

**Налаштування серіалізатора:**

```csharp
private static readonly JsonSerializerOptions JsonOptions = new()
{
    WriteIndented = true,                                        // форматований вивід
    PropertyNameCaseInsensitive = true,                          // гнучке читання
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping        // кирилиця як є
};
```

---

## Приклад JSON-файлу

```json
[
  {
    "id": 1,
    "title": "Let It Be",
    "artist": {
      "id": 1,
      "name": "The Beatles",
      "country": "Велика Британія",
      "birth_year": 1960
    },
    "genre": "Rock",
    "duration_seconds": 243,
    "release_year": 1970
  },
  {
    "id": 2,
    "title": "bad guy",
    "artist": {
      "id": 2,
      "name": "Billie Eilish",
      "country": "США",
      "birth_year": 2001
    },
    "genre": "Pop",
    "duration_seconds": 194,
    "release_year": 2019
  }
]
```

---

## Демонстрація роботи

```
=== Лабораторна робота №28 — Серіалізація JSON ===

[ Додавання пісень ]
  + Додано: [Song #1] "Let It Be" — The Beatles | Rock | 4:03 | 1970
  + Додано: [Song #2] "bad guy" — Billie Eilish | Pop | 3:14 | 2019
  + Додано: [Song #3] "Believer" — Imagine Dragons | Rock | 3:24 | 2017
  + Додано: [Song #4] "Rolling in the Deep" — Adele | Pop | 3:48 | 2010
  + Додано: [Song #5] "Yesterday" — The Beatles | Rock | 2:05 | 1965

[ Збереження у JSON-файл ]
  Збережено 5 пісень → data/songs.json

[ Завантаження з JSON-файлу ]
  Завантажено 5 пісень ← data/songs.json

[ Усі пісні після завантаження ]
──────────────────────────────────────────────────────────────────────
  [Song #1] "Let It Be" — The Beatles | Rock | 4:03 | 1970
  [Song #2] "bad guy" — Billie Eilish | Pop | 3:14 | 2019
  [Song #3] "Believer" — Imagine Dragons | Rock | 3:24 | 2017
  [Song #4] "Rolling in the Deep" — Adele | Pop | 3:48 | 2010
  [Song #5] "Yesterday" — The Beatles | Rock | 2:05 | 1965

[ Пошук за Id = 3 ]
──────────────────────────────────────────────────────────────────────
  Знайдено: [Song #3] "Believer" — Imagine Dragons | Rock | 3:24 | 2017
```

---

## Запуск

```powershell
cd lab28v5
dotnet run
```

Файл `data/songs.json` створюється автоматично.

---

## Використані технології

| Технологія | Версія | Призначення |
|---|---|---|
| .NET | 8.0 | Платформа |
| System.Text.Json | вбудований | JSON-серіалізація |
| System.IO | вбудований | Робота з файлами |

---

## Висновок

У ході виконання лабораторної роботи реалізовано три класи предметної області (`Genre`, `Artist`, `Song`) з атрибутами `[JsonPropertyName]`, `[JsonIgnore]` та `[JsonConverter]`, а також репозиторій `SongRepository` з асинхронною JSON-серіалізацією через `JsonSerializer.SerializeAsync` / `DeserializeAsync`. Продемонстровано повний цикл: створення об'єктів → збереження у файл → завантаження → пошук → вивід.
