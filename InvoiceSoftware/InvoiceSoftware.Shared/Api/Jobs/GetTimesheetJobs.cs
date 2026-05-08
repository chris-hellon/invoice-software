using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Jobs;

namespace InvoiceSoftware.Shared.Api.Jobs;

[Route("api/jobs/timesheet/{WeekStart}")]
public class GetTimesheetJobs : IGet<List<TimesheetJobDto>>
{
    public DateOnly WeekStart { get; set; }
}
