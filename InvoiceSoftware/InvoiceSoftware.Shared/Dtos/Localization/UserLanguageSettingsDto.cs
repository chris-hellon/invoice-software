namespace InvoiceSoftware.Shared.Dtos.Localization;

public record UserLanguageSettingsDto(
    string DefaultLanguage,
    string InvoiceLanguage,
    string EstimateLanguage);
