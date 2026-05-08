using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.BusinessProfile;

namespace InvoiceSoftware.Shared.Api.BusinessProfile;

[Route("api/business-profile")]
public class GetBusinessProfile : IGet<BusinessProfileDto?>
{
}
