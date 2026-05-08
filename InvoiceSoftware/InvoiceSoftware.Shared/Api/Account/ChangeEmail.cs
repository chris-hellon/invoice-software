using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Account;

[Route("api/account/email/change")]
public class ChangeEmail : IPost
{
    [BodyParam]
    public string NewEmail { get; set; } = "";
}
