using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Invoices;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Invoices;

public class UpdateInvoiceHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<UpdateInvoice>
{
    public async Task<HttpResult> Handle(UpdateInvoice request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var invoice = await db.Invoices.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (invoice is null) return HttpResult.NotFound();

        invoice.UpdateDetails(request.IssueDate, request.DueDate, request.TaxRate, request.Notes);

        // Remove time entries
        if (request.RemovedTimeEntryIds?.Count > 0)
        {
            var toUnlink = await db.TimeEntries
                .Where(e => request.RemovedTimeEntryIds.Contains(e.Id))
                .ToListAsync(cancellationToken);
            foreach (var entry in toUnlink)
            {
                entry.UnlinkFromInvoice();
            }
        }

        // Add time entries
        if (request.AddedTimeEntryIds?.Count > 0)
        {
            var toLink = await db.TimeEntries
                .Where(e => request.AddedTimeEntryIds.Contains(e.Id))
                .ToListAsync(cancellationToken);
            foreach (var entry in toLink)
            {
                entry.LinkToInvoice(request.Id);
            }
        }

        // Remove expenses
        if (request.RemovedExpenseIds?.Count > 0)
        {
            var toUnlink = await db.Expenses
                .Where(e => request.RemovedExpenseIds.Contains(e.Id))
                .ToListAsync(cancellationToken);
            foreach (var expense in toUnlink)
            {
                expense.UnmarkBilled();
            }
        }

        // Add expenses
        if (request.AddedExpenseIds?.Count > 0)
        {
            var toLink = await db.Expenses
                .Where(e => request.AddedExpenseIds.Contains(e.Id))
                .ToListAsync(cancellationToken);
            foreach (var expense in toLink)
            {
                expense.MarkAsBilled(request.Id);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
