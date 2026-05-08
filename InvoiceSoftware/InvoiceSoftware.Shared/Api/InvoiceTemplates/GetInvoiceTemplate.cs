using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.InvoiceTemplates;

namespace InvoiceSoftware.Shared.Api.InvoiceTemplates;

[Route("api/invoice-templates/{Id}")]
public class GetInvoiceTemplate : IGet<InvoiceTemplateDto?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
