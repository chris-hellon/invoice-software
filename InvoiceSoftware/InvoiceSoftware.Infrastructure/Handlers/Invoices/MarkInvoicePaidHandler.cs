using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Invoices;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Invoices;

public class MarkInvoicePaidHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<MarkInvoicePaid>
{
    public async Task<HttpResult> Handle(MarkInvoicePaid request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var invoice = await db.Invoices.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (invoice is null) return HttpResult.NotFound();

        invoice.MarkAsPaid(request.PaidDate);
        db.Entry(invoice).State = EntityState.Modified;
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
