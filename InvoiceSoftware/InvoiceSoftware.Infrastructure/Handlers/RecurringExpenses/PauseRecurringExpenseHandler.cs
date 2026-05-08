using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.RecurringExpenses;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.RecurringExpenses;

public class PauseRecurringExpenseHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<PauseRecurringExpense>
{
    public async Task<HttpResult> Handle(PauseRecurringExpense request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var recurringExpense = await db.RecurringExpenses.FirstOrDefaultAsync(re => re.Id == request.Id, cancellationToken);
        if (recurringExpense is null) return HttpResult.NotFound();

        recurringExpense.Pause();

        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
