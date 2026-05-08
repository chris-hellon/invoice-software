using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Estimates;

[Route("api/estimates/{Id}/send")]
public class SendEstimate : IPost
{
    [RouteParam]
    public Guid Id { get; init; }
}
