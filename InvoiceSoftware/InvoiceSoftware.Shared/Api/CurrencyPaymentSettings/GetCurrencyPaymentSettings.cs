using BlazorUtils.EasyApi;
using InvoiceSoftware.Shared.Dtos.CurrencyPaymentSettings;

namespace InvoiceSoftware.Shared.Api.CurrencyPaymentSettings;

[Route("api/currency-payment-settings/{CurrencyCode}")]
public class GetCurrencyPaymentSettings : IGet<CurrencyPaymentSettingsDto?>
{
    [RouteParam]
    public string CurrencyCode { get; init; } = null!;
}
