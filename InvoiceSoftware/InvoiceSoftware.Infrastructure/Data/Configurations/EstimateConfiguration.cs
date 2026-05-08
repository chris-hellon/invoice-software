using InvoiceSoftware.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceSoftware.Infrastructure.Data.Configurations;

public class EstimateConfiguration : IEntityTypeConfiguration<Estimate>
{
    public void Configure(EntityTypeBuilder<Estimate> builder)
    {
        builder.ToTable("Estimates");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(e => e.EstimateNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(e => e.EstimateNumber)
            .IsUnique();

        builder.Property(e => e.EstimateDate).IsRequired();

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(e => e.TaxRate)
            .HasPrecision(5, 2);

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        builder.Property(e => e.Notes)
            .HasMaxLength(2000);

        builder.Property(e => e.Terms)
            .HasMaxLength(4000);

        builder.HasOne(e => e.Client)
            .WithMany()
            .HasForeignKey(e => e.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ConvertedInvoice)
            .WithMany()
            .HasForeignKey(e => e.ConvertedInvoiceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(e => e.LineItems)
            .WithOne(li => li.Estimate)
            .HasForeignKey(li => li.EstimateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.ClientId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.PublicAccessToken);
    }
}
