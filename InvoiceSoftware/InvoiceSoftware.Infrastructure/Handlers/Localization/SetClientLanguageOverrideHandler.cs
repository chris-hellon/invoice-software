using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Entities;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Localization;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Localization;

public class SetClientLanguageOverrideHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<SetClientLanguageOverride>
{
    public async Task<HttpResult> Handle(SetClientLanguageOverride request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var client = await db.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientId && c.UserId == userId, cancellationToken);

        if (client == null)
            return HttpResult.NotFound();

        if (!Enum.TryParse<SupportedLanguage>(request.PreferredLanguage, out var language))
            return HttpResult.NotFound();

        var existingOverride = await db.ClientLanguageSettings
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ClientId == request.ClientId, cancellationToken);

        if (existingOverride != null)
        {
            existingOverride.Update(language, request.UseForInvoices, request.UseForEstimates);
        }
        else
        {
            var newOverride = ClientLanguageSetting.Create(
                userId,
                request.ClientId,
                language,
                request.UseForInvoices,
                request.UseForEstimates);
            db.ClientLanguageSettings.Add(newOverride);
        }

        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
