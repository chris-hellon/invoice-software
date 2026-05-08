namespace InvoiceSoftware.Shared.Dtos.CurrencyPaymentSettings;

public record CurrencyPaymentSettingsDto(
    Guid Id,
    string CurrencyCode,
    string? BankName,
    string? BankAccountName,
    string? BankAccountNumber,
    string? BankSortCode,
    string? BankIban,
    string? BankSwift,
    string? VietQrBankCode);
