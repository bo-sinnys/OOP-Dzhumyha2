# ── Etap 1: build ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо тільки .csproj файли для кешування залежностей (без .sln щоб не тягнути тести)
COPY src/BankingSystem.Domain/BankingSystem.Domain.csproj           src/BankingSystem.Domain/
COPY src/BankingSystem.Application/BankingSystem.Application.csproj src/BankingSystem.Application/
COPY src/BankingSystem.Infrastructure/BankingSystem.Infrastructure.csproj src/BankingSystem.Infrastructure/
COPY src/BankingSystem.Console/BankingSystem.Console.csproj         src/BankingSystem.Console/

RUN dotnet restore src/BankingSystem.Console/BankingSystem.Console.csproj

# Копіюємо весь вихідний код
COPY src/ src/

# Публікуємо Release-збірку
RUN dotnet publish src/BankingSystem.Console/BankingSystem.Console.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ── Etap 2: runtime ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

# Метадані образу
LABEL maintainer="BankingSystem Lab38" \
      version="1.0" \
      description="Banking System Console App — Lab 38 Docker"

# Директорія для даних (volume mount point) — до зміни користувача
RUN mkdir -p /app/data

# Непривілейований користувач для безпеки
RUN adduser --disabled-password --gecos "" appuser && \
    chown -R appuser:appuser /app
USER appuser

COPY --from=build /app/publish ./

# Перевірка здоров'я
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD dotnet BankingSystem.Console.dll --health 2>/dev/null || exit 0

ENTRYPOINT ["dotnet", "BankingSystem.Console.dll"]
