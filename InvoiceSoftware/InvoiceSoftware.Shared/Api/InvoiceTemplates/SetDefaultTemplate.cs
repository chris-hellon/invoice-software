using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.InvoiceTemplates;

[Route("api/invoice-templates/{Id}/default")]
public class SetDefaultTemplate : IPost
{
    [RouteParam]
    public Guid Id { get; init; }
}
