using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Entities;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.ProjectSections;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.ProjectSections;

public class CreateProjectSectionHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<CreateProjectSection, Guid>
{
    public async Task<HttpResult<Guid>> Handle(CreateProjectSection request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var maxOrder = await db.ProjectSections
            .Where(s => s.ProjectId == request.ProjectId)
            .MaxAsync(s => (int?)s.Order, cancellationToken) ?? -1;

        var section = ProjectSection.Create(
            request.ProjectId,
            request.Name,
            request.Description,
            maxOrder + 1);

        db.ProjectSections.Add(section);
        await db.SaveChangesAsync(cancellationToken);

        return HttpResult<Guid>.Created(section.Id);
    }
}
