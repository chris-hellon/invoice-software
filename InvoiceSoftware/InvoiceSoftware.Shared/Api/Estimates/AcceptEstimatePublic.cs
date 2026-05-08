using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Estimates;

[Route("api/estimates/public/{Token}/accept")]
public class AcceptEstimatePublic : IPost
{
    [RouteParam]
    public Guid Token { get; init; }

    [BodyParam]
    public string? SignerName { get; init; }

    [BodyParam]
    public string? SignerEmail { get; init; }
}
