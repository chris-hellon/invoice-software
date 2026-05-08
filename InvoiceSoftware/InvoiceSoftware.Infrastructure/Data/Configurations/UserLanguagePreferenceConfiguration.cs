using InvoiceSoftware.Domain.Entities;
using InvoiceSoftware.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceSoftware.Infrastructure.Data.Configurations;

public class UserLanguagePreferenceConfiguration : IEntityTypeConfiguration<UserLanguagePreference>
{
    public void Configure(EntityTypeBuilder<UserLanguagePreference> builder)
    {
        builder.ToTable("UserLanguagePreferences");

        builder.HasKey(ulp => ulp.Id);

        builder.Property(ulp => ulp.UserId)
            .HasMaxLength(450)
            .IsRequired();

        builder.HasIndex(ulp => ulp.UserId)
            .IsUnique();

        builder.Property(ulp => ulp.DefaultLanguage)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(SupportedLanguage.English);

        builder.Property(ulp => ulp.InvoiceLanguage)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(SupportedLanguage.English);

        builder.Property(ulp => ulp.EstimateLanguage)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(SupportedLanguage.English);
    }
}
