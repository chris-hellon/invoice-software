using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Invoices;

[Route("api/invoices")]
public class CreateInvoice : IPost<Guid>
{
    [BodyParam]
    public Guid ClientId { get; init; }

    [BodyParam]
    public List<Guid> TimeEntryIds { get; init; } = [];

    [BodyParam]
    public List<Guid> ExpenseIds { get; init; } = [];

    [BodyParam]
    public DateOnly IssueDate { get; init; }

    [BodyParam]
    public DateOnly DueDate { get; init; }

    [BodyParam]
    public decimal TaxRate { get; init; }

    [BodyParam]
    public string? Notes { get; init; }

    [BodyParam]
    public Guid? TemplateId { get; init; }
}
