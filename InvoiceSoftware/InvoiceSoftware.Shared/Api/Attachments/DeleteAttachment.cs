using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Attachments;

[Route("api/attachments/{Id}")]
public class DeleteAttachment : IDelete
{
    [RouteParam]
    public Guid Id { get; init; }
}
