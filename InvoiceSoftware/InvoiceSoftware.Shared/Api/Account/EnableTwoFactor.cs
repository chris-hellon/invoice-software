using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Account;

namespace InvoiceSoftware.Shared.Api.Account;

[Route("api/account/2fa/enable")]
public class EnableTwoFactor : IPost<EnableTwoFactorResultDto>
{
    [BodyParam]
    public string Code { get; set; } = "";
}
