using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Server;
using InvoiceSoftware.Infrastructure.Data;
using InvoiceSoftware.Shared.Api.Account;
using InvoiceSoftware.Shared.Dtos.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InvoiceSoftware.Infrastructure.Handlers.Account;

public class GetProfileHandler(
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor) : IHandle<GetProfile, ProfileDto>
{
    public async Task<HttpResult<ProfileDto>> Handle(GetProfile request, CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User!);
        if (user is null)
            return HttpResult<ProfileDto>.Unauthorized();

        var username = await userManager.GetUserNameAsync(user) ?? "";
        var email = await userManager.GetEmailAsync(user) ?? "";
        var phoneNumber = await userManager.GetPhoneNumberAsync(user);
        var hasPassword = await userManager.HasPasswordAsync(user);
        var twoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user);
        var passkeys = await userManager.GetPasskeysAsync(user);

        return HttpResult<ProfileDto>.Ok(new ProfileDto(
            username,
            email,
            phoneNumber,
            hasPassword,
            twoFactorEnabled,
            passkeys.Count
        ));
    }
}
