using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.DAL.Entities;

namespace OnlineLearningPlatform.DAL.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(c => c.Title).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(2000).IsRequired();
        builder.Property(c => c.Price).HasColumnType("decimal(18,2)");
        builder.Property(c => c.Level).HasMaxLength(50);
        builder.Property(c => c.Category).HasMaxLength(100);

        builder.HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstructorProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
