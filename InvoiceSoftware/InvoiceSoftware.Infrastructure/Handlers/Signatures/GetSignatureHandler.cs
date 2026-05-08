using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Signatures;
using InvoiceSoftware.Shared.Dtos.Signatures;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Signatures;

public class GetSignatureHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<GetSignature, SignatureDto?>
{
    public async Task<HttpResult<SignatureDto?>> Handle(GetSignature request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult<SignatureDto?>.Unauthorized();

        if (!Enum.TryParse<LinkedEntityType>(request.LinkedEntityType, out var entityType))
            return HttpResult<SignatureDto?>.Ok(null);

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var signature = await db.DigitalSignatures
            .Where(s => s.LinkedEntityType == entityType && s.LinkedEntityId == request.LinkedEntityId)
            .OrderByDescending(s => s.SignedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (signature == null)
            return HttpResult<SignatureDto?>.Ok(null);

        var dto = new SignatureDto(
            signature.Id,
            signature.LinkedEntityType.ToString(),
            signature.LinkedEntityId,
            signature.SignerName,
            signature.SignerEmail,
            signature.SignedAt,
            signature.IsValid,
            signature.InvalidationReason,
            signature.GetSignatureAsBase64());

        return HttpResult<SignatureDto?>.Ok(dto);
    }
}
