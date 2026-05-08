using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Expenses;
using InvoiceSoftware.Shared.Dtos.Expenses;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Expenses;

public class GetExpenseHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<GetExpense, ExpenseDetailDto?>
{
    public async Task<HttpResult<ExpenseDetailDto?>> Handle(GetExpense request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var expense = await db.Expenses
            .Include(e => e.Client)
            .Include(e => e.Project)
            .Include(e => e.Invoice)
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (expense is null) return HttpResult<ExpenseDetailDto?>.NotFound();

        var result = new ExpenseDetailDto(
            expense.Id,
            expense.Category.ToString(),
            expense.MerchantName,
            expense.ExpenseDate,
            expense.PaymentMethod.ToString(),
            expense.Amount.Amount,
            expense.Amount.Currency,
            expense.TaxAmount,
            expense.IsTaxInclusive,
            expense.MerchantTaxNumber,
            expense.GroupName,
            expense.Notes,
            expense.IsReimbursable,
            expense.IsBillable,
            expense.IsBilled,
            expense.ClientId,
            expense.Client?.Name,
            expense.ProjectId,
            expense.Project?.Name,
            expense.InvoiceId,
            expense.Invoice?.InvoiceNumber,
            expense.RecurringExpenseId,
            expense.CreatedAt,
            expense.ModifiedAt);

        return HttpResult<ExpenseDetailDto?>.Ok(result);
    }
}
