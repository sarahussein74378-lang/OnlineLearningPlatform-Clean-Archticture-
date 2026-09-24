using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.DAL.Entities;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.DAL.Data.Configurations;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.Property(c => c.CertificateNumber).HasMaxLength(50).IsRequired();
        builder.HasIndex(c => c.CertificateNumber).IsUnique();
        builder.Property(c => c.PdfPath).HasMaxLength(500);

        builder.HasOne(c => c.Student)
            .WithMany(s => s.Certificates)
            .HasForeignKey(c => c.StudentProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
