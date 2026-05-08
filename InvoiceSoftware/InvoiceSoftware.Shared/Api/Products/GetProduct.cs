using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Products;

namespace InvoiceSoftware.Shared.Api.Products;

[Route("api/products/{Id}")]
public class GetProduct : IGet<ProductDetailDto?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
