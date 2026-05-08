using InvoiceSoftware.Shared.Dtos.CurrencyPaymentSettings;
using InvoiceSoftware.Shared.Dtos.InvoiceTemplates;

namespace InvoiceSoftware.Shared.Dtos.Invoices;

public record PublicInvoiceDto(
    Guid Id,
    string InvoiceNumber,
    string ClientName,
    string? ClientCompanyName,
    string? ClientEmail,
    string? ClientAddress,
    string Currency,
    DateOnly IssueDate,
    DateOnly DueDate,
    DateOnly? PaidDate,
    string Status,
    decimal Subtotal,
    decimal TaxRate,
    decimal TaxAmount,
    decimal Total,
    string? Notes,
    List<InvoiceLineItemDto> LineItems,
    List<InvoiceExpenseLineItemDto> ExpenseLineItems,
    // Business profile info for display
    string? BusinessName,
    string? BusinessAddress,
    string? BusinessEmail,
    string? BusinessPhone,
    byte[]? BusinessLogo,
    string? TaxNumber,
    string? RegistrationNumber,
    // Payment options
    string? PayPalMeUsername,
    string? WiseEmail,
    string? RevolutUsername,
    // Currency-specific bank details
    CurrencyPaymentSettingsDto? CurrencyPaymentSettings,
    // Template styling
    InvoiceTemplateDto? Template = null);
