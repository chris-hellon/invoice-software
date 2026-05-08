using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.ValueObjects;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.BusinessProfile;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.BusinessProfile;

public class SaveBusinessProfileHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<SaveBusinessProfile>
{
    public async Task<HttpResult> Handle(SaveBusinessProfile request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var profile = await db.BusinessProfiles.FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);

        Address? address = null;
        if (!string.IsNullOrWhiteSpace(request.Street))
        {
            address = new Address(
                request.Street,
                request.City ?? "",
                request.State ?? "",
                request.PostalCode ?? "",
                request.Country ?? "");
        }

        if (profile is null)
        {
            profile = Domain.Entities.BusinessProfile.Create(userId, request.CompanyName, request.Email);
            profile.Update(
                request.CompanyName,
                request.TradingName,
                request.Email,
                request.Phone,
                request.Website,
                address,
                request.TaxNumber,
                request.RegistrationNumber,
                request.BankName,
                request.BankAccountName,
                request.BankAccountNumber,
                request.BankSortCode,
                request.BankIban,
                request.BankSwift,
                request.DefaultCurrency,
                request.DefaultPaymentTermsDays,
                request.InvoiceNotes,
                request.InvoiceFooter,
                request.InvoiceEmailSubject,
                request.InvoiceEmailBody,
                request.PayPalMeUsername,
                request.WiseEmail,
                request.RevolutUsername,
                request.VietQrBankCode);
            db.BusinessProfiles.Add(profile);
        }
        else
        {
            profile.Update(
                request.CompanyName,
                request.TradingName,
                request.Email,
                request.Phone,
                request.Website,
                address,
                request.TaxNumber,
                request.RegistrationNumber,
                request.BankName,
                request.BankAccountName,
                request.BankAccountNumber,
                request.BankSortCode,
                request.BankIban,
                request.BankSwift,
                request.DefaultCurrency,
                request.DefaultPaymentTermsDays,
                request.InvoiceNotes,
                request.InvoiceFooter,
                request.InvoiceEmailSubject,
                request.InvoiceEmailBody,
                request.PayPalMeUsername,
                request.WiseEmail,
                request.RevolutUsername,
                request.VietQrBankCode);
        }

        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
