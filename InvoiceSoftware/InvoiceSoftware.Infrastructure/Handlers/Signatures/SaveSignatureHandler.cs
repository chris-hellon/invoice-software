using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Entities;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Signatures;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Signatures;

public class SaveSignatureHandler(IDbContextFactory<ApplicationDbContext> dbFactory, IHttpContextAccessor httpContextAccessor)
    : IHandle<SaveSignature, Guid>
{
    public async Task<HttpResult<Guid>> Handle(SaveSignature request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<LinkedEntityType>(request.LinkedEntityType, out var entityType))
            return HttpResult<Guid>.NotFound();

        // Get signer's IP address
        var ipAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        try
        {
            var signature = DigitalSignature.Create(
                entityType,
                request.LinkedEntityId,
                request.SignatureData,
                request.SignerName,
                request.SignerEmail,
                ipAddress);

            db.DigitalSignatures.Add(signature);
            await db.SaveChangesAsync(cancellationToken);
            return HttpResult<Guid>.Created(signature.Id);
        }
        catch (Domain.Exceptions.DomainException)
        {
            return HttpResult<Guid>.NotFound();
        }
    }
}
