using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Localization;

namespace InvoiceSoftware.Shared.Api.Localization;

[Route("api/localization/settings")]
public class GetUserLanguageSettings : IGet<UserLanguageSettingsDto?>
{
}
