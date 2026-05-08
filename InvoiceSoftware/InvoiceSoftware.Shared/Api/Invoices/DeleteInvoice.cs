using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Invoices;

[Route("api/invoices/{Id}")]
public class DeleteInvoice : IDelete
{
    [RouteParam]
    public Guid Id { get; init; }
}
