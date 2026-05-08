using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.RecurringExpenses;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.RecurringExpenses;

public class ResumeRecurringExpenseHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<ResumeRecurringExpense>
{
    public async Task<HttpResult> Handle(ResumeRecurringExpense request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var recurringExpense = await db.RecurringExpenses.FirstOrDefaultAsync(re => re.Id == request.Id, cancellationToken);
        if (recurringExpense is null) return HttpResult.NotFound();

        recurringExpense.Resume();

        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
