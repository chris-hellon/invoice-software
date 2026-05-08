using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.InvoiceTemplates;

namespace InvoiceSoftware.Shared.Api.InvoiceTemplates;

[Route("api/invoice-templates")]
public class GetInvoiceTemplates : IGet<List<InvoiceTemplateDto>>
{
    [QueryStringParam]
    public bool IncludeSystem { get; init; } = true;
}
