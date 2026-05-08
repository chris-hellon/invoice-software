using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Account;

namespace InvoiceSoftware.Shared.Api.Account;

[Route("api/account/2fa/recovery-codes")]
public class GenerateRecoveryCodes : IPost<RecoveryCodesDto>;
