using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.ClientPortal;

namespace InvoiceSoftware.Shared.Api.ClientPortal;

[Route("api/portal/{Token}")]
public class GetClientPortalSummary : IGet<ClientPortalSummaryDto?>
{
    [RouteParam]
    public Guid Token { get; init; }
}
