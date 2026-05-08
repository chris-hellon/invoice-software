using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Entities;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.BankImport;
using InvoiceSoftware.Shared.Dtos.BankImport;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace InvoiceSoftware.Infrastructure.Handlers.BankImport;

public class ImportBankStatementHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<ImportBankStatement, ImportResultDto>
{
    public async Task<HttpResult<ImportResultDto>> Handle(
        ImportBankStatement request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult<ImportResultDto>.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var errors = new List<string>();
        var importedCount = 0;
        var skippedDuplicates = 0;
        var autoMatched = 0;

        // Convert byte array to string
        var fileContent = Encoding.UTF8.GetString(request.FileContent);

        // Parse CSV content
        var lines = fileContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2)
        {
            return HttpResult<ImportResultDto>.Ok(new ImportResultDto(0, 0, 0, 0, new List<string> { "File is empty or has no data rows" }));
        }

        // Get existing transactions to check for duplicates
        var existingHashes = await db.BankTransactions
            .Where(bt => bt.UserId == userId)
            .Select(bt => bt.Description + bt.TransactionDate + bt.Amount.Amount)
            .ToHashSetAsync(cancellationToken);

        var rowNumber = 0;
        foreach (var line in lines.Skip(1)) // Skip header
        {
            rowNumber++;
            var columns = ParseCsvLine(line);

            if (columns.Count < 3)
            {
                errors.Add($"Row {rowNumber}: Invalid format");
                continue;
            }

            try
            {
                // Expected format: Date, Description, Amount, [Reference], [Currency]
                if (!DateOnly.TryParse(columns[0], out var date))
                {
                    errors.Add($"Row {rowNumber}: Invalid date format");
                    continue;
                }

                var description = columns[1].Trim();
                if (!decimal.TryParse(columns[2], NumberStyles.Currency, CultureInfo.InvariantCulture, out var amount))
                {
                    errors.Add($"Row {rowNumber}: Invalid amount format");
                    continue;
                }

                var reference = columns.Count > 3 ? columns[3].Trim() : null;
                var currency = columns.Count > 4 ? columns[4].Trim() : request.Currency;

                // Check for duplicates
                var hash = description + date + amount;
                if (existingHashes.Contains(hash))
                {
                    skippedDuplicates++;
                    continue;
                }

                var transaction = BankTransaction.Create(
                    userId,
                    date,
                    description,
                    amount,
                    currency,
                    request.FileName,
                    rowNumber,
                    reference);

                db.BankTransactions.Add(transaction);
                existingHashes.Add(hash);
                importedCount++;
            }
            catch (Exception ex)
            {
                errors.Add($"Row {rowNumber}: {ex.Message}");
            }
        }

        await db.SaveChangesAsync(cancellationToken);

        return HttpResult<ImportResultDto>.Ok(new ImportResultDto(
            rowNumber,
            importedCount,
            skippedDuplicates,
            autoMatched,
            errors));
    }

    private static List<string> ParseCsvLine(string line)
    {
        var result = new List<string>();
        var inQuotes = false;
        var current = "";

        foreach (var c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }
        result.Add(current);

        return result;
    }
}
