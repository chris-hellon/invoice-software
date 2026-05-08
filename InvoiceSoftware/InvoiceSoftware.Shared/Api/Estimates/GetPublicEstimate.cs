using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Estimates;

namespace InvoiceSoftware.Shared.Api.Estimates;

[Route("api/estimates/public/{Token}")]
public class GetPublicEstimate : IGet<PublicEstimateDto?>
{
    [RouteParam]
    public Guid Token { get; init; }
}
