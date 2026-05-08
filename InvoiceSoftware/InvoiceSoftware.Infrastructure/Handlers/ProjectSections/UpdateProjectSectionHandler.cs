using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.ProjectSections;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.ProjectSections;

public class UpdateProjectSectionHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<UpdateProjectSection>
{
    public async Task<HttpResult> Handle(UpdateProjectSection request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var section = await db.ProjectSections.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        if (section is null) return HttpResult.NotFound();

        section.Update(request.Name, request.Description);
        await db.SaveChangesAsync(cancellationToken);

        return HttpResult.NoContent();
    }
}
