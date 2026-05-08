using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Estimates;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Estimates;

public class RejectEstimatePublicHandler(IDbContextFactory<ApplicationDbContext> dbFactory)
    : IHandle<RejectEstimatePublic>
{
    public async Task<HttpResult> Handle(RejectEstimatePublic request, CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var estimate = await db.Estimates
            .FirstOrDefaultAsync(e => e.PublicAccessToken == request.Token, cancellationToken);

        if (estimate == null)
            return HttpResult.NotFound();

        estimate.Reject();
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
