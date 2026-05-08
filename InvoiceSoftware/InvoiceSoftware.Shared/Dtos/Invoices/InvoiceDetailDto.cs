using InvoiceSoftware.Shared.Dtos.InvoiceTemplates;

namespace InvoiceSoftware.Shared.Dtos.Invoices;

public record InvoiceDetailDto(
    Guid Id,
    string InvoiceNumber,
    Guid ClientId,
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
    Guid? PublicAccessToken = null,
    Guid? TemplateId = null,
    InvoiceTemplateDto? Template = null);
