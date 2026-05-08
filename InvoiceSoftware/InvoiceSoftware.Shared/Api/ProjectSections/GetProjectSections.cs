using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.ProjectSections;

namespace InvoiceSoftware.Shared.Api.ProjectSections;

[Route("api/projects/{ProjectId}/sections")]
public class GetProjectSections : IGet<List<ProjectSectionDto>>
{
    [RouteParam]
    public Guid ProjectId { get; init; }
}
