using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Invoices;

[Route("api/invoices/{Id}/pdf")]
public class GetInvoicePdf : IGet<byte[]>
{
    [RouteParam]
    public Guid Id { get; init; }
}
