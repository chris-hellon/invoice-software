using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.ValueObjects;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Clients;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Clients;

public class UpdateClientHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<UpdateClient>
{
    public async Task<HttpResult> Handle(UpdateClient request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var client = await db.Clients.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (client is null) return HttpResult.NotFound();

        Address? billingAddress = null;
        if (!string.IsNullOrEmpty(request.Street))
        {
            billingAddress = new Address(
                request.Street,
                request.City ?? "",
                request.State ?? "",
                request.PostalCode ?? "",
                request.Country ?? "");
        }

        client.Update(
            request.Name,
            request.Email,
            request.DefaultHourlyRate,
            request.Currency,
            request.CompanyName,
            request.Phone,
            billingAddress);

        if (request.IsActive)
            client.Activate();
        else
            client.Deactivate();

        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
