using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.RecurringInvoices;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.RecurringInvoices;

public class UpdateRecurringInvoiceHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<UpdateRecurringInvoice>
{
    public async Task<HttpResult> Handle(UpdateRecurringInvoice request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var recurring = await db.RecurringInvoices
            .Include(r => r.LineItems)
            .FirstOrDefaultAsync(r => r.Id == request.Id && r.UserId == userId, cancellationToken);

        if (recurring == null)
            return HttpResult.NotFound();

        if (!Enum.TryParse<RecurrenceFrequency>(request.Frequency, out var frequency))
            return HttpResult.NotFound();

        recurring.Update(
            request.TemplateName,
            request.FrequencyInterval,
            frequency,
            request.StartDate,
            request.EndDate,
            request.DueDays,
            request.TaxRate,
            request.Notes,
            request.Terms,
            request.Footer);

        // Clear and re-add line items
        recurring.ClearLineItems();

        foreach (var item in request.LineItems)
        {
            recurring.AddLineItem(
                item.Description,
                item.Quantity,
                item.UnitPrice,
                item.ProductId);
        }

        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
