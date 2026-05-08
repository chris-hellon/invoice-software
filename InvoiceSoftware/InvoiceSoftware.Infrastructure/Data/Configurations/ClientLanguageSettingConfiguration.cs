using InvoiceSoftware.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceSoftware.Infrastructure.Data.Configurations;

public class ClientLanguageSettingConfiguration : IEntityTypeConfiguration<ClientLanguageSetting>
{
    public void Configure(EntityTypeBuilder<ClientLanguageSetting> builder)
    {
        builder.ToTable("ClientLanguageSettings");

        builder.HasKey(cls => cls.Id);

        builder.Property(cls => cls.UserId)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(cls => cls.PreferredLanguage)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(cls => cls.Client)
            .WithMany()
            .HasForeignKey(cls => cls.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cls => cls.UserId);
        builder.HasIndex(cls => new { cls.UserId, cls.ClientId })
            .IsUnique();
    }
}
