using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Estimates;

namespace InvoiceSoftware.Shared.Api.Estimates;

[Route("api/estimates/{Id}")]
public class GetEstimate : IGet<EstimateDetailDto?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
