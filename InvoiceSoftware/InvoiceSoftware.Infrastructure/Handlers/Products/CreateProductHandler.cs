using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Domain.Entities;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Products;
using InvoiceSoftware.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSoftware.Infrastructure.Handlers.Products;

public class CreateProductHandler(IDbContextFactory<ApplicationDbContext> dbFactory, ICurrentUserService currentUserService)
    : IHandle<CreateProduct, Guid>
{
    public async Task<HttpResult<Guid>> Handle(CreateProduct request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return HttpResult<Guid>.Unauthorized();

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var product = Product.Create(
            userId,
            request.Name,
            request.UnitPrice,
            request.Currency,
            request.Description,
            request.DefaultQuantity,
            request.Category,
            request.Sku,
            request.TaxRate);

        db.Products.Add(product);
        await db.SaveChangesAsync(cancellationToken);
        return HttpResult<Guid>.Created(product.Id);
    }
}
