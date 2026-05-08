using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Invoices;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Invoices;

public class SendInvoiceHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<SendInvoice>
{
    public async Task<HttpResult> Handle(SendInvoice request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var invoice = await db.Invoices
            .Include(i => i.Client)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice is null) return HttpResult.NotFound();

        invoice.Send();
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
