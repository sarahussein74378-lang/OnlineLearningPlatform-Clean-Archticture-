using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.DAL.Entities;

namespace OnlineLearningPlatform.DAL.Data.Configurations;

public class QuizSubmissionAnswerConfiguration : IEntityTypeConfiguration<QuizSubmissionAnswer>
{
    public void Configure(EntityTypeBuilder<QuizSubmissionAnswer> builder)
    {
        builder.HasOne(sa => sa.QuizSubmission)
            .WithMany(s => s.SubmissionAnswers)
            .HasForeignKey(sa => sa.QuizSubmissionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sa => sa.Question)
            .WithMany()
            .HasForeignKey(sa => sa.QuestionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(sa => sa.SelectedAnswer)
            .WithMany()
            .HasForeignKey(sa => sa.SelectedAnswerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
