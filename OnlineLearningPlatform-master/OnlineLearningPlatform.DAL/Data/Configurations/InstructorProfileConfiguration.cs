using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.DAL.Entities;

namespace OnlineLearningPlatform.DAL.Data.Configurations;

public class InstructorProfileConfiguration : IEntityTypeConfiguration<InstructorProfile>
{
    public void Configure(EntityTypeBuilder<InstructorProfile> builder)
    {
        builder.Property(i => i.Rating).HasColumnType("decimal(3,2)");
        builder.Property(i => i.Bio).HasMaxLength(2000);
        builder.Property(i => i.Expertise).HasMaxLength(500);

        builder.HasOne(i => i.User)
            .WithOne(u => u.InstructorProfile)
            .HasForeignKey<InstructorProfile>(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
