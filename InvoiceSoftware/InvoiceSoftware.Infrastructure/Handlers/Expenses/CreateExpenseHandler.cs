using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Entities;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Expenses;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Expenses;

public class CreateExpenseHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<CreateExpense, Guid>
{
    public async Task<HttpResult<Guid>> Handle(CreateExpense request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var category = Enum.TryParse<ExpenseCategory>(request.Category, out var c) ? c : ExpenseCategory.Other;
        var paymentMethod = Enum.TryParse<PaymentMethod>(request.PaymentMethod, out var p) ? p : PaymentMethod.Other;

        var expense = Expense.Create(
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

        db.Expenses.Add(expense);
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult<Guid>.Created(expense.Id);
    }
}
