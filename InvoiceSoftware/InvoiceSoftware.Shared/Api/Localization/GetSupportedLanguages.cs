using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.Localization;

namespace InvoiceSoftware.Shared.Api.Localization;

[Route("api/localization/languages")]
public class GetSupportedLanguages : IGet<List<LanguageDto>>
{
}
