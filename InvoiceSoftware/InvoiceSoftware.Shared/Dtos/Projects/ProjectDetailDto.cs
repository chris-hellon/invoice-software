using InvoiceSoftware.Shared.Dtos.Jobs;
using InvoiceSoftware.Shared.Dtos.ProjectSections;

namespace InvoiceSoftware.Shared.Dtos.Projects;

public record ProjectDetailDto(
    Guid Id,
    Guid ClientId,
    string ClientName,
    string Name,
    string? Description,
    string Status,
    DateOnly? StartDate,
    DateOnly? EndDate,
    decimal? HourlyRateOverride,
    List<JobDto> Jobs,
    List<ProjectSectionDto> Sections);
