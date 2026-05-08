using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.BusinessProfile;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.BusinessProfile;

public class DeleteBusinessLogoHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<DeleteBusinessLogo>
{
    public async Task<HttpResult> Handle(DeleteBusinessLogo request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var profile = await db.BusinessProfiles.FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);
        if (profile is null) return HttpResult.NoContent();

        profile.UpdateLogo(null, null);
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
