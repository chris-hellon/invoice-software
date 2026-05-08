using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Expenses;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Expenses;

public class DeleteExpenseHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<DeleteExpense>
{
    public async Task<HttpResult> Handle(DeleteExpense request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var expense = await db.Expenses.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (expense is null)
            return HttpResult.NotFound();

        db.Expenses.Remove(expense);
        await db.SaveChangesAsync(cancellationToken);

        return HttpResult.NoContent();
    }
}
