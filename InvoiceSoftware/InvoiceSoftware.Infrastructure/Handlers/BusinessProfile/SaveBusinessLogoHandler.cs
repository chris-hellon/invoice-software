using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.BusinessProfile;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.BusinessProfile;

public class SaveBusinessLogoHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<SaveBusinessLogo>
{
    public async Task<HttpResult> Handle(SaveBusinessLogo request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var profile = await db.BusinessProfiles.FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);
        if (profile is null)
            return HttpResult.NotFound();

        profile.UpdateLogo(request.Logo, request.ContentType);
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
