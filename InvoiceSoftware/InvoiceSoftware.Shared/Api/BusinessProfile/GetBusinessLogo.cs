using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.BusinessProfile;

[Route("api/business-profile/logo")]
public class GetBusinessLogo : IGet<byte[]?>
{
}
