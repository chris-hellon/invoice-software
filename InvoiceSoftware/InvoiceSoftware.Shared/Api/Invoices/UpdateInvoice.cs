using BlazorUtils.EasyApi;

namespace InvoiceSoftware.Shared.Api.Invoices;

[Route("api/invoices/{Id}")]
public class UpdateInvoice : IPut
{
    [RouteParam]
    public Guid Id { get; init; }

    [BodyParam]
    public DateOnly IssueDate { get; init; }

    [BodyParam]
    public DateOnly DueDate { get; init; }

    [BodyParam]
    public decimal TaxRate { get; init; }

    [BodyParam]
    public string? Notes { get; init; }

    [BodyParam]
    public List<Guid>? AddedTimeEntryIds { get; init; }

    [BodyParam]
    public List<Guid>? RemovedTimeEntryIds { get; init; }

    [BodyParam]
    public List<Guid>? AddedExpenseIds { get; init; }

    [BodyParam]
    public List<Guid>? RemovedExpenseIds { get; init; }
}
