using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Projects;

namespace InvoiceSoftware.Shared.Api.Projects;

[Route("api/projects/{Id}")]
public class GetProject : IGet<ProjectDetailDto?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
