using InvoiceSoftware.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceSoftware.Infrastructure.Data.Configurations;

public class CurrencyPaymentSettingsConfiguration : IEntityTypeConfiguration<CurrencyPaymentSettings>
{
    public void Configure(EntityTypeBuilder<CurrencyPaymentSettings> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => new { c.UserId, c.CurrencyCode }).IsUnique();

        builder.Property(c => c.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(c => c.CurrencyCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(c => c.BankName).HasMaxLength(200);
        builder.Property(c => c.BankAccountName).HasMaxLength(200);
        builder.Property(c => c.BankAccountNumber).HasMaxLength(50);
        builder.Property(c => c.BankSortCode).HasMaxLength(20);
        builder.Property(c => c.BankIban).HasMaxLength(50);
        builder.Property(c => c.BankSwift).HasMaxLength(20);
        builder.Property(c => c.VietQrBankCode).HasMaxLength(20);
    }
}
