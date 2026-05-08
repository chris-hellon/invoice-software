using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Account;

namespace InvoiceSoftware.Shared.Api.Account;

[Route("api/account/2fa")]
public class GetTwoFactorStatus : IGet<TwoFactorStatusDto>;
