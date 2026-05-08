using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.CurrencyPaymentSettings;

namespace InvoiceSoftware.Shared.Api.CurrencyPaymentSettings;

[Route("api/currency-payment-settings")]
public class GetAllCurrencyPaymentSettings : IGet<List<CurrencyPaymentSettingsDto>>
{
}
