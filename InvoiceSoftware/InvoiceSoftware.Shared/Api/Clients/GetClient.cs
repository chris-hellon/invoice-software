using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Clients;

namespace InvoiceSoftware.Shared.Api.Clients;

[Route("api/clients/{Id}")]
public class GetClient : IGet<ClientDetailDto?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
