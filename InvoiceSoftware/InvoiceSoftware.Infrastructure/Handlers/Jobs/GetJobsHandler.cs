using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api;
using InvoiceSoftware.Shared.Api.Jobs;
using InvoiceSoftware.Shared.Dtos.Jobs;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Jobs;

public class GetJobsHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<GetJobs, PaginatedResponse<JobDto>>
{
    public async Task<HttpResult<PaginatedResponse<JobDto>>> Handle(GetJobs request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var query = db.Jobs
            .Include(j => j.Project)
                .ThenInclude(p => p.Client)
            .Include(j => j.Section)
            .AsQueryable();

        if (request.ProjectId.HasValue)
            query = query.Where(j => j.ProjectId == request.ProjectId.Value);

        // Status filter
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<JobStatus>(request.Status, out var status))
            query = query.Where(j => j.Status == status);

        // Search filter
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(j =>
                j.Name.ToLower().Contains(search) ||
                (j.Description != null && j.Description.ToLower().Contains(search)) ||
                j.Project.Name.ToLower().Contains(search) ||
                j.Project.Client.Name.ToLower().Contains(search));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var jobs = await query
            .OrderBy(j => j.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = jobs.Select(j => new JobDto(
            j.Id,
            j.ProjectId,
            j.Project.Name,
            j.Project.ClientId,
            j.Project.Client.Name,
            j.SectionId,
            j.Section?.Name,
            j.Name,
            j.Description,
            j.Status.ToString(),
            j.EstimatedHours?.Value,
            j.HourlyRateOverride?.Value)).ToList();

        var result = new PaginatedResponse<JobDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };

        return HttpResult<PaginatedResponse<JobDto>>.Ok(result);
    }
}
