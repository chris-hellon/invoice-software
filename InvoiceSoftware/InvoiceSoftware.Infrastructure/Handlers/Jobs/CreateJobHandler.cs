using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Entities;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Jobs;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Jobs;

public class CreateJobHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<CreateJob, Guid>
{
    public async Task<HttpResult<Guid>> Handle(CreateJob request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var priority = Enum.TryParse<JobPriority>(request.Priority, out var p) ? p : JobPriority.Medium;

        var job = Job.Create(
            request.ProjectId,
            request.Name,
            request.Description,
            request.Notes,
            request.EstimatedHours,
            request.HourlyRateOverride,
            priority,
            request.StartDate,
            request.DueDate,
            request.SectionId);

        db.Jobs.Add(job);
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult<Guid>.Created(job.Id);
    }
}
