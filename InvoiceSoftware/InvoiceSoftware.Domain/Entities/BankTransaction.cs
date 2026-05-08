using InvoiceSoftware.Domain.Common;
using InvoiceSoftware.Domain.Enums;
using InvoiceSoftware.Domain.Exceptions;
using InvoiceSoftware.Domain.ValueObjects;

namespace InvoiceSoftware.Domain.Entities;

public class BankTransaction : AggregateRoot, IAuditableEntity
{
    public string UserId { get; private set; } = null!;
    public DateOnly TransactionDate { get; private set; }
    public string Description { get; private set; } = null!;
    public Money Amount { get; private set; } = null!;
    public string? Reference { get; private set; }
    public string? BankAccountName { get; private set; }

    public Guid? MatchedInvoiceId { get; private set; }
    public MatchConfidence MatchConfidence { get; private set; } = MatchConfidence.None;
    public string? MatchNotes { get; private set; }

    public DateTime ImportedAt { get; private set; }
    public string SourceFileName { get; private set; } = null!;
    public int SourceRowNumber { get; private set; }

    public bool IsMatched => MatchedInvoiceId.HasValue;
    public bool IsIgnored { get; private set; }
    public string? IgnoreReason { get; private set; }

    public Invoice? MatchedInvoice { get; private set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }

    private BankTransaction() { }

    public static BankTransaction Create(
        string userId,
        DateOnly transactionDate,
        string description,
        decimal amount,
        string currency,
        string sourceFileName,
        int sourceRowNumber,
        string? reference = null,
        string? bankAccountName = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Transaction description is required");

        return new BankTransaction
        {
            UserId = userId,
            TransactionDate = transactionDate,
            Description = description,
            Amount = new Money(amount, currency),
            Reference = reference,
            BankAccountName = bankAccountName,
            SourceFileName = sourceFileName,
            SourceRowNumber = sourceRowNumber,
            ImportedAt = DateTime.UtcNow
        };
    }

    public void MatchToInvoice(Guid invoiceId, MatchConfidence confidence, string? notes = null)
    {
        if (IsIgnored)
            throw new DomainException("Cannot match an ignored transaction");
        if (IsMatched)
            throw new DomainException("Transaction is already matched to an invoice");

        MatchedInvoiceId = invoiceId;
        MatchConfidence = confidence;
        MatchNotes = notes;
    }

    public void UnmatchInvoice()
    {
        MatchedInvoiceId = null;
        MatchConfidence = MatchConfidence.None;
        MatchNotes = null;
    }

    public void Ignore(string? reason = null)
    {
        if (IsMatched)
            throw new DomainException("Cannot ignore a matched transaction");

        IsIgnored = true;
        IgnoreReason = reason;
    }

    public void Unignore()
    {
        IsIgnored = false;
        IgnoreReason = null;
    }
}
