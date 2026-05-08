using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Invoices;

namespace InvoiceSoftware.Shared.Api.Invoices;

[Route("api/invoices/{Id}")]
public class GetInvoice : IGet<InvoiceDetailDto?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
