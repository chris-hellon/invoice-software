using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Estimates;

[Route("api/estimates/{Id}/reject")]
public class RejectEstimate : IPost
{
    [RouteParam]
    public Guid Id { get; init; }
}
