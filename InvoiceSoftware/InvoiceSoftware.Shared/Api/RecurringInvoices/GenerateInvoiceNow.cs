using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.RecurringInvoices;

[Route("api/recurring-invoices/{Id}/generate")]
public class GenerateInvoiceNow : IPost<Guid?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
