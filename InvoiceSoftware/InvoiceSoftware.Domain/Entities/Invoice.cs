using InvoiceSoftware.Domain.Common;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Domain.Exceptions;

namespace InvoiceSoftware.Domain.Entities;

public class Invoice : AggregateRoot, IAuditableEntity
{
    public string InvoiceNumber { get; private set; } = null!;
    public Guid ClientId { get; private set; }
    public DateOnly IssueDate { get; private set; }
    public DateOnly DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; } = InvoiceStatus.Draft;
    public decimal TaxRate { get; private set; }
    public string? Notes { get; private set; }
    public DateOnly? PaidDate { get; private set; }
    public string Currency { get; private set; } = "USD";
    public Guid? PublicAccessToken { get; private set; }
    public Guid? TemplateId { get; private set; }

    public Client Client { get; private set; } = null!;
    public InvoiceTemplate? Template { get; private set; }

    // Simple link to time entries - just IDs, no complex entity management
    private readonly List<TimeEntry> _timeEntries = [];
    public IReadOnlyCollection<TimeEntry> TimeEntries => _timeEntries.AsReadOnly();

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }

    private Invoice() { }

    public static Invoice Create(
        string invoiceNumber,
        Guid clientId,
        DateOnly issueDate,
        DateOnly dueDate,
        decimal taxRate = 0,
        string? notes = null,
        string currency = "USD")
    {
        return new Invoice
        {
            InvoiceNumber = invoiceNumber,
            ClientId = clientId,
            IssueDate = issueDate,
            DueDate = dueDate,
            TaxRate = taxRate,
            Notes = notes,
            Currency = currency,
            PublicAccessToken = Guid.NewGuid()
        };
    }

    public void UpdateDetails(
        DateOnly issueDate,
        DateOnly dueDate,
        decimal taxRate,
        string? notes,
        Guid? templateId = null)
    {
        if (Status != InvoiceStatus.Draft)
            throw new DomainException("Cannot modify a non-draft invoice");

        IssueDate = issueDate;
        DueDate = dueDate;
        TaxRate = taxRate;
        Notes = notes;
        TemplateId = templateId;
    }

    public void SetTemplate(Guid? templateId)
    {
        TemplateId = templateId;
    }

    public void Send()
    {
        if (Status != InvoiceStatus.Draft)
            throw new DomainException("Only draft invoices can be sent");

        Status = InvoiceStatus.Sent;

        // Generate public access token if not already set
        PublicAccessToken ??= Guid.NewGuid();
    }

    public void MarkAsPaid(DateOnly paidDate)
    {
        if (Status == InvoiceStatus.Paid)
            throw new DomainException("Invoice is already marked as paid");
        if (Status == InvoiceStatus.Void)
            throw new DomainException("Cannot mark a voided invoice as paid");

        Status = InvoiceStatus.Paid;
        PaidDate = paidDate;
    }

    public void MarkAsOverdue()
    {
        if (Status == InvoiceStatus.Sent)
            Status = InvoiceStatus.Overdue;
    }

    public void Void()
    {
        if (Status == InvoiceStatus.Paid)
            throw new DomainException("Cannot void a paid invoice");

        Status = InvoiceStatus.Void;
    }
}
