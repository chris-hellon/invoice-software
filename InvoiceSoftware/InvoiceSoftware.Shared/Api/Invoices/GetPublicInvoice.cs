using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Invoices;

namespace InvoiceSoftware.Shared.Api.Invoices;

/// <summary>
/// Gets an invoice by its public access token. No authentication required.
/// </summary>
[Route("/api/invoices/public/{Token}")]
public class GetPublicInvoice : IGet<PublicInvoiceDto?>
{
    [RouteParam]
    public Guid Token { get; set; }
}
