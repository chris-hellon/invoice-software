using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Clients;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Clients;

public class DeleteClientHandler(IDbContextFactory<ApplicationDbContext> dbFactory) : IHandle<DeleteClient>
{
    public async Task<HttpResult> Handle(DeleteClient request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var client = await db.Clients.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (client is null) return HttpResult.NotFound();

        client.Deactivate();
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
