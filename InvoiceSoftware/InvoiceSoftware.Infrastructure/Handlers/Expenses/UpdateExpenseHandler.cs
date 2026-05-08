using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Expenses;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Expenses;

public class UpdateExpenseHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<UpdateExpense>
{
    public async Task<HttpResult> Handle(UpdateExpense request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var expense = await db.Expenses.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        if (expense is null) return HttpResult.NotFound();

        var category = Enum.TryParse<ExpenseCategory>(request.Category, out var c) ? c : ExpenseCategory.Other;
        var paymentMethod = Enum.TryParse<PaymentMethod>(request.PaymentMethod, out var p) ? p : PaymentMethod.Other;

        expense.Update(
            category,
            request.MerchantName,
            request.ExpenseDate,
            paymentMethod,
            request.Amount,
            request.Currency,
            request.TaxAmount,
            request.IsTaxInclusive,
            request.MerchantTaxNumber,
            request.GroupName,
            request.Notes,
            request.IsReimbursable,
            request.IsBillable,
            request.ClientId,
            request.ProjectId);

        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
