using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Entities;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.RecurringInvoices;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.RecurringInvoices;

public class CreateRecurringInvoiceHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<CreateRecurringInvoice, Guid>
{
    public async Task<HttpResult<Guid>> Handle(CreateRecurringInvoice request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult<Guid>.Unauthorized();

        if (!Enum.TryParse<RecurrenceFrequency>(request.Frequency, out var frequency))
            return HttpResult<Guid>.NotFound();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var recurring = RecurringInvoice.Create(
            userId,
            request.ClientId,
            request.TemplateName,
            request.Currency,
            request.FrequencyInterval,
            frequency,
            request.StartDate,
            request.DueDays,
            request.EndDate,
            request.TaxRate,
            request.Notes,
            request.Terms,
            request.Footer);

        foreach (var item in request.LineItems)
        {
            recurring.AddLineItem(
                item.Description,
                item.Quantity,
                item.UnitPrice,
                item.ProductId);
        }

        db.RecurringInvoices.Add(recurring);
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult<Guid>.Created(recurring.Id);
    }
}
