using InvoiceSoftware.Domain.Common;

namespace InvoiceSoftware.Domain.Entities;

public class CurrencyPaymentSettings : IAuditableEntity
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; } = null!;
    public string CurrencyCode { get; private set; } = null!;

    // Bank Details
    public string? BankName { get; private set; }
    public string? BankAccountName { get; private set; }
    public string? BankAccountNumber { get; private set; }
    public string? BankSortCode { get; private set; }
    public string? BankIban { get; private set; }
    public string? BankSwift { get; private set; }

    // Currency-specific payment options
    public string? VietQrBankCode { get; private set; } // Only for VND

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }

    private CurrencyPaymentSettings() { }

    public static CurrencyPaymentSettings Create(string userId, string currencyCode)
    {
        return new CurrencyPaymentSettings
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CurrencyCode = currencyCode
        };
    }

    public void Update(
        string? bankName,
        string? bankAccountName,
        string? bankAccountNumber,
        string? bankSortCode,
        string? bankIban,
        string? bankSwift,
        string? vietQrBankCode)
    {
        BankName = bankName;
        BankAccountName = bankAccountName;
        BankAccountNumber = bankAccountNumber;
        BankSortCode = bankSortCode;
        BankIban = bankIban;
        BankSwift = bankSwift;
        VietQrBankCode = vietQrBankCode;
    }
}
