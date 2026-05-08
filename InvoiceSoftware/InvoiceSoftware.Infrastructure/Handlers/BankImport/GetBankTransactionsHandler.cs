using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api;
using InvoiceSoftware.Shared.Api.BankImport;
using InvoiceSoftware.Shared.Dtos.BankImport;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.BankImport;

public class GetBankTransactionsHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<GetBankTransactions, PaginatedResponse<BankTransactionDto>>
{
    public async Task<HttpResult<PaginatedResponse<BankTransactionDto>>> Handle(
        GetBankTransactions request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult<PaginatedResponse<BankTransactionDto>>.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var query = db.BankTransactions
            .Include(bt => bt.MatchedInvoice)
            .Where(bt => bt.UserId == userId);

        // Filter by match status
        if (request.IsMatched.HasValue)
            query = query.Where(bt => bt.IsMatched == request.IsMatched.Value);

        if (request.IsIgnored.HasValue)
            query = query.Where(bt => bt.IsIgnored == request.IsIgnored.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var transactions = await query
            .OrderByDescending(bt => bt.TransactionDate)
            .ThenByDescending(bt => bt.ImportedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = transactions.Select(bt => new BankTransactionDto(
            bt.Id,
            bt.TransactionDate,
            bt.Description,
            bt.Amount.Amount,
            bt.Amount.Currency,
            bt.Reference,
            bt.BankAccountName,
            bt.MatchedInvoiceId,
            bt.MatchedInvoice?.InvoiceNumber,
            bt.MatchConfidence.ToString(),
            bt.MatchNotes,
            bt.IsMatched,
            bt.IsIgnored,
            bt.IgnoreReason,
            bt.ImportedAt,
            bt.SourceFileName)).ToList();

        return HttpResult<PaginatedResponse<BankTransactionDto>>.Ok(
            new PaginatedResponse<BankTransactionDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            });
    }
}
