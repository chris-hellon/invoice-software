using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Account;
using InvoiceSoftware.Shared.Dtos.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InvoiceSoftware.Infrastructure.Handlers.Account;

public class GetTwoFactorStatusHandler(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IHttpContextAccessor httpContextAccessor) : IHandle<GetTwoFactorStatus, TwoFactorStatusDto>
{
    public async Task<HttpResult<TwoFactorStatusDto>> Handle(GetTwoFactorStatus request, CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User!);
        if (user is null)
            return HttpResult<TwoFactorStatusDto>.Unauthorized();

        var is2faEnabled = await userManager.GetTwoFactorEnabledAsync(user);
        var isMachineRemembered = await signInManager.IsTwoFactorClientRememberedAsync(user);
        var recoveryCodesLeft = await userManager.CountRecoveryCodesAsync(user);
        var hasAuthenticator = await userManager.GetAuthenticatorKeyAsync(user) is not null;

        return HttpResult<TwoFactorStatusDto>.Ok(new TwoFactorStatusDto(
            is2faEnabled,
            isMachineRemembered,
            recoveryCodesLeft,
            hasAuthenticator));
    }
}
