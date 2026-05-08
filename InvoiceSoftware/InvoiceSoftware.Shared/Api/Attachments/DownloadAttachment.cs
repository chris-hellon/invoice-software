using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Attachments;

[Route("api/attachments/{Id}/download")]
public class DownloadAttachment : IGet<byte[]?>
{
    [RouteParam]
    public Guid Id { get; init; }
}
