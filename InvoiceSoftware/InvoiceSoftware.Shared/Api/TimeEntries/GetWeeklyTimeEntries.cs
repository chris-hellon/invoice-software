using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.TimeEntries;

namespace InvoiceSoftware.Shared.Api.TimeEntries;

[Route("api/timeentries/week/{WeekStart}")]
public class GetWeeklyTimeEntries : IGet<List<TimeEntryDto>>
{
    [RouteParam]
    public DateOnly WeekStart { get; init; }
}
