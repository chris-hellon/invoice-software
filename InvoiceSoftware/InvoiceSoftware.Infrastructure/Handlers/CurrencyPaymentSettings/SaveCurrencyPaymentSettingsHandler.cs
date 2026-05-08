using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.CurrencyPaymentSettings;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.CurrencyPaymentSettings;

public class SaveCurrencyPaymentSettingsHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<SaveCurrencyPaymentSettings>
{
    public async Task<HttpResult> Handle(SaveCurrencyPaymentSettings request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var settings = await db.CurrencyPaymentSettings
            .FirstOrDefaultAsync(c => c.UserId == userId && c.CurrencyCode == request.CurrencyCode, cancellationToken);

        if (settings is null)
        {
            settings = Domain.Entities.CurrencyPaymentSettings.Create(userId, request.CurrencyCode);
            db.CurrencyPaymentSettings.Add(settings);
        }

        settings.Update(
            request.BankName,
            request.BankAccountName,
            request.BankAccountNumber,
            request.BankSortCode,
            request.BankIban,
            request.BankSwift,
            request.VietQrBankCode);

        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
