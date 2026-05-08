using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.TimeEntries;

namespace InvoiceSoftware.Shared.Api.TimeEntries;

[Route("api/timeentries/unbilled/{ClientId}")]
public class GetUnbilledTimeEntries : IGet<List<UnbilledTimeEntryDto>>
{
    [RouteParam]
    public Guid ClientId { get; init; }
}
