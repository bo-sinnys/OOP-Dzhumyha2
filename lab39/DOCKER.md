# Docker — BankingSystem

Повна інструкція з контейнеризації консольного застосунку BankingSystem.

---

## Швидкий старт

```bash
# 1. Зібрати образ
docker build -t bankingsystem:v1 .

# 2. Запустити контейнер (інтерактивний режим)
docker run -it --rm bankingsystem:v1
```

---

## Як побудувати образ

### Стандартний (runtime ~200 MB)
```bash
docker build -t bankingsystem:v1 .
```

### Alpine-варіант (~100 MB)
```bash
docker build -f Dockerfile.alpine -t bankingsystem:alpine .
```

### Порівняння розмірів

| Підхід              | Dockerfile          | Очікуваний розмір |
|---------------------|---------------------|-------------------|
| Single-stage (SDK)  | `FROM sdk:8.0`      | ~900 MB           |
| Multi-stage runtime | `Dockerfile`        | ~200 MB           |
| Multi-stage alpine  | `Dockerfile.alpine` | ~100 MB           |

Перевірити фактичні розміри:
```bash
docker images bankingsystem
```

**Чому така різниця?**  
- `sdk`-образ містить компілятор, NuGet-кеш, весь .NET toolchain — ~900 MB  
- `runtime`-образ містить лише .NET runtime для запуску — ~200 MB  
- `alpine`-варіант базується на мінімальному Alpine Linux замість Debian — ~100 MB  

Multi-stage build дозволяє збирати проєкт у SDK-образі, а до фінального образу копіювати лише скомпільовані артефакти.

---

## Як запустити контейнер

### Інтерактивно (консольний застосунок)
```bash
docker run -it --rm bankingsystem:v1
```

### З volume для файлового I/O
```bash
docker run -it --rm \
  -v $(pwd)/data:/app/data \
  bankingsystem:v1
```

### Через Docker Compose
```bash
docker compose up --build    # зібрати та запустити
docker compose down          # зупинити та прибрати контейнери
```

---

## Volumes

| Шлях на хості | Шлях у контейнері | Призначення                      |
|---------------|-------------------|----------------------------------|
| `./data`      | `/app/data`       | Логи shutdown, файли стану       |

При зупинці контейнера (`SIGTERM`) застосунок записує мітку часу до `./data/shutdown.log`.

---

## Змінні середовища

| Змінна                                | Значення за замовч. | Опис                        |
|---------------------------------------|---------------------|-----------------------------|
| `DOTNET_ENVIRONMENT`                  | `Production`        | Середовище запуску .NET     |
| `DOTNET_SYSTEM_GLOBALIZATION_INVARIANT` | `false`           | Підтримка Unicode/UTF-8     |

---

## Health Check

Dockerfile містить вбудовану перевірку стану:
```
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3
    CMD dotnet BankingSystem.Console.dll --health
```

Перевірити статус:
```bash
docker inspect --format='{{.State.Health.Status}}' banking-app
```

---

## Graceful Shutdown

Застосунок обробляє сигнал `SIGTERM` (надсилається Docker при `docker stop`):
- перехоплюється `Console.CancelKeyPress` та `AppDomain.CurrentDomain.ProcessExit`
- перед виходом записує лог у `/app/data/shutdown.log`
- виводить повідомлення про коректне завершення

```bash
docker stop banking-app    # Docker надішле SIGTERM → застосунок завершиться коректно
```

---

## Безпека

- Контейнер запускається від непривілейованого користувача `appuser` (`USER appuser`)
- `.dockerignore` виключає `bin/`, `obj/`, `.git/`, тестові проєкти, секрети

---

## Корисні команди

```bash
# Переглянути запущені контейнери
docker ps

# Логи контейнера
docker logs banking-app

# Зайти всередину контейнера
docker exec -it banking-app sh

# Видалити образ
docker rmi bankingsystem:v1
```
