using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.RecurringInvoices;

[Route("api/recurring-invoices/{Id}")]
public class DeleteRecurringInvoice : IDelete
{
    [RouteParam]
    public Guid Id { get; init; }
}
