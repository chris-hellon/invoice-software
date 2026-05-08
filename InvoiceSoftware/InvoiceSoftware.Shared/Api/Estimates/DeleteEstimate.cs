using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Estimates;

[Route("api/estimates/{Id}")]
public class DeleteEstimate : IDelete
{
    [RouteParam]
    public Guid Id { get; init; }
}
