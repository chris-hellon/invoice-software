using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Account;

namespace InvoiceSoftware.Shared.Api.Account;

[Route("api/account/2fa/authenticator-key")]
public class GetAuthenticatorKey : IGet<AuthenticatorKeyDto>;
