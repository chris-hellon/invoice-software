using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Jobs;

namespace InvoiceSoftware.Shared.Api.Jobs;

[Route("api/jobs/{Id}")]
public class GetJob : IGet<JobDetailDto?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
