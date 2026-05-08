using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.ProjectSections;

[Route("api/projects/sections/{Id}")]
public class DeleteProjectSection : IDelete
{
    [RouteParam]
    public Guid Id { get; init; }
}
