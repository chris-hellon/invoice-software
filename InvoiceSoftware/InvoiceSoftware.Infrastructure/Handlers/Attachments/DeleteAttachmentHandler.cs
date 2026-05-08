using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Attachments;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Attachments;

public class DeleteAttachmentHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<DeleteAttachment>
{
    public async Task<HttpResult> Handle(DeleteAttachment request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var attachment = await db.Attachments
            .FirstOrDefaultAsync(a => a.Id == request.Id && a.UserId == userId, cancellationToken);

        if (attachment == null)
            return HttpResult.NotFound();

        db.Attachments.Remove(attachment);
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult.NoContent();
    }
}
