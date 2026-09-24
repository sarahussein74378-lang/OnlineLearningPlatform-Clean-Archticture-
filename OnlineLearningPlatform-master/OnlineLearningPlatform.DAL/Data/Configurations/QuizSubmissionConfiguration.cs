using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.DAL.Entities;

namespace OnlineLearningPlatform.DAL.Data.Configurations;

public class QuizSubmissionConfiguration : IEntityTypeConfiguration<QuizSubmission>
{
    public void Configure(EntityTypeBuilder<QuizSubmission> builder)
    {
        builder.HasOne(s => s.Quiz)
            .WithMany(q => q.Submissions)
            .HasForeignKey(s => s.QuizId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Student)
            .WithMany()
            .HasForeignKey(s => s.StudentProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
