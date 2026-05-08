using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.CurrencyPaymentSettings;

[Route("api/currency-payment-settings")]
public class SaveCurrencyPaymentSettings : IPut
{
    [BodyParam]
    public string CurrencyCode { get; init; } = null!;

    [BodyParam]
    public string? BankName { get; init; }

    [BodyParam]
    public string? BankAccountName { get; init; }

    [BodyParam]
    public string? BankAccountNumber { get; init; }

    [BodyParam]
    public string? BankSortCode { get; init; }

    [BodyParam]
    public string? BankIban { get; init; }

    [BodyParam]
    public string? BankSwift { get; init; }

    [BodyParam]
    public string? VietQrBankCode { get; init; }
}
