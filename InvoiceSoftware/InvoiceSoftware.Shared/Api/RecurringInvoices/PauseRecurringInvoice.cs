using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.RecurringInvoices;

[Route("api/recurring-invoices/{Id}/pause")]
public class PauseRecurringInvoice : IPost
{
    [RouteParam]
    public Guid Id { get; init; }
}
