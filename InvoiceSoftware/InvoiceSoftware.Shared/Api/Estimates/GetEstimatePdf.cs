using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Estimates;

[Route("api/estimates/{Id}/pdf")]
public class GetEstimatePdf : IGet<byte[]?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
