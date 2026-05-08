using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.BusinessProfile;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.BusinessProfile;

public class GetBusinessLogoHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<GetBusinessLogo, byte[]?>
{
    public async Task<HttpResult<byte[]?>> Handle(GetBusinessLogo request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId)) return HttpResult<byte[]?>.Ok(null);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var profile = await db.BusinessProfiles.FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);
        return HttpResult<byte[]?>.Ok(profile?.Logo);
    }
}
