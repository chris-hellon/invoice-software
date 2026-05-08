using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace InvoiceSoftware.Infrastructure.Handlers.Account;

public class ResetAuthenticatorHandler(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IHttpContextAccessor httpContextAccessor,
    ILogger<ResetAuthenticatorHandler> logger) : IHandle<ResetAuthenticator>
{
    public async Task<HttpResult> Handle(ResetAuthenticator request, CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User!);
        if (user is null)
            return HttpResult.Unauthorized();

        await userManager.SetTwoFactorEnabledAsync(user, false);
        await userManager.ResetAuthenticatorKeyAsync(user);
        logger.LogInformation("User with ID '{UserId}' has reset their authenticator app key.", user.Id);

        await signInManager.RefreshSignInAsync(user);

        return HttpResult.NoContent();
    }
}
