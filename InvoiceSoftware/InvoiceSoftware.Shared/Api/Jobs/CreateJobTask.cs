using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Jobs;

namespace InvoiceSoftware.Shared.Api.Jobs;

[Route("api/jobs/{JobId}/tasks")]
public class CreateJobTask : IPost<JobTaskDto>
{
    [RouteParam]
    public Guid JobId { get; init; }

    [BodyParam]
    public string Title { get; init; } = null!;

    [BodyParam]
    public string? Description { get; init; }
}
