using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Jobs;

[Route("api/jobs/{Id}")]
public class DeleteJob : IDelete
{
    [RouteParam]
    public Guid Id { get; init; }
}
