using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Dashboard;

namespace InvoiceSoftware.Shared.Api.Dashboard;

[Route("api/dashboard/top-clients")]
public class GetTopClients : IGet<List<TopClientDto>>
{
    [QueryStringParam]
    public int Count { get; init; } = 5;

    [QueryStringParam]
    public string Period { get; init; } = "year";
}
