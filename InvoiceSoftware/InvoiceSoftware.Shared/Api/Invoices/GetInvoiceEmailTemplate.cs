using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Invoices;

[Route("api/invoices/{Id}/email-template")]
public class GetInvoiceEmailTemplate : IGet<string?>
{
    [RouteParam] public Guid Id { get; init; }
}
