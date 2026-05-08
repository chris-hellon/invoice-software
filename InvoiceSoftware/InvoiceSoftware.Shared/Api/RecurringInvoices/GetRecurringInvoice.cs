using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.RecurringInvoices;

namespace InvoiceSoftware.Shared.Api.RecurringInvoices;

[Route("api/recurring-invoices/{Id}")]
public class GetRecurringInvoice : IGet<RecurringInvoiceDetailDto?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
