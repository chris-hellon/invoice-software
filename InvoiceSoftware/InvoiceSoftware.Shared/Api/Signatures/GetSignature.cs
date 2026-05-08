using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Signatures;

namespace InvoiceSoftware.Shared.Api.Signatures;

[Route("api/signatures")]
public class GetSignature : IGet<SignatureDto?>
{
    [QueryStringParam]
    public string LinkedEntityType { get; init; } = null!;

    [QueryStringParam]
    public Guid LinkedEntityId { get; init; }
}
