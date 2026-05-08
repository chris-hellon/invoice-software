using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Entities;
using InvoiceSoftware.Domain.ValueObjects;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Clients;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Clients;

public class CreateClientHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService) : IHandle<CreateClient, Guid>
{
    public async Task<HttpResult<Guid>> Handle(CreateClient request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult<Guid>.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
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

        var client = Client.Create(
            userId,
            request.Name,
            request.Email,
            request.DefaultHourlyRate,
            request.Currency,
            request.CompanyName,
            request.Phone,
            billingAddress);

        db.Clients.Add(client);
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult<Guid>.Created(client.Id);
    }
}
