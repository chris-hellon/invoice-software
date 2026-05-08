using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Clients;
using InvoiceSoftware.Shared.Dtos.Clients;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Clients;

public class GetClientHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<GetClient, ClientDetailDto?>
{
    public async Task<HttpResult<ClientDetailDto?>> Handle(GetClient request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var client = await db.Clients
            .Include(c => c.Projects)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (client is null) return HttpResult<ClientDetailDto?>.NotFound();

        var result = new ClientDetailDto(
            client.Id,
            client.Name,
            client.CompanyName,
            client.Email.Value,
            client.Phone?.Value,
            client.DefaultHourlyRate.Value,
            client.Currency,
            client.IsActive,
            client.BillingAddress?.Street1,
            client.BillingAddress?.City,
            client.BillingAddress?.State,
            client.BillingAddress?.PostalCode,
            client.BillingAddress?.Country,
            client.Projects.Count,
            client.Projects.Count(p => p.Status == ProjectStatus.Active));

        return HttpResult<ClientDetailDto?>.Ok(result);
    }
}
