using BankingSystem.Domain.Entities;
using System.Text;

namespace BankingSystem.Application.Services;

/// <summary>
/// Сервіс для експорту виписки по рахунку у форматі CSV.
/// Issue #1 — feat: add CSV export for transaction history
/// </summary>
public class CsvExportService
{
    /// <summary>
    /// Формує CSV-рядок з усіх транзакцій рахунку.
    /// </summary>
    public string ExportTransactions(IReadOnlyList<Transaction> transactions)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Date,Type,Amount,Description");

        foreach (var t in transactions)
        {
            var date = t.OccurredAt.ToString("yyyy-MM-dd HH:mm:ss");
            var type = t.Type.ToString();
            var amount = t.Amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            var description = $"\"{t.Description.Replace("\"", "\"\"")}\"";

            sb.AppendLine($"{date},{type},{amount},{description}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Зберігає CSV у файл за вказаним шляхом.
    /// </summary>
    public async Task ExportToFileAsync(IReadOnlyList<Transaction> transactions, string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        var csv = ExportTransactions(transactions);
        await File.WriteAllTextAsync(filePath, csv, Encoding.UTF8);
    }
}
