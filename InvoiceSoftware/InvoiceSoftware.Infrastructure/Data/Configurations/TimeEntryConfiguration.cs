using InvoiceSoftware.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceSoftware.Infrastructure.Data.Configurations;

public class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
{
    public void Configure(EntityTypeBuilder<TimeEntry> builder)
    {
        builder.ToTable("TimeEntries");

        builder.HasKey(te => te.Id);

        builder.Property(te => te.Date)
            .IsRequired();

        builder.OwnsOne(te => te.Hours, hours =>
        {
            hours.Property(h => h.Value)
                .HasColumnName("Hours")
                .HasPrecision(5, 2)
                .IsRequired();
        });

        builder.Property(te => te.Description)
            .HasMaxLength(500);

        builder.Property(te => te.UserId)
            .HasMaxLength(450)
            .IsRequired();

        // IsBilled is now a computed property, so we ignore it
        builder.Ignore(te => te.IsBilled);

        builder.HasIndex(te => te.JobId);
        builder.HasIndex(te => te.Date);
        builder.HasIndex(te => te.UserId);
        builder.HasIndex(te => te.InvoiceId);
        builder.HasIndex(te => new { te.UserId, te.Date });
    }
}
