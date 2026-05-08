using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Entities;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Projects;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Projects;

public class CreateProjectHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<CreateProject, Guid>
{
    public async Task<HttpResult<Guid>> Handle(CreateProject request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var project = Project.Create(
            request.ClientId,
            request.Name,
            request.Description,
            request.StartDate,
            request.EndDate,
            request.HourlyRateOverride);

        db.Projects.Add(project);
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult<Guid>.Created(project.Id);
    }
}
