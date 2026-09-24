namespace OnlineLearningPlatform.DAL.Entities;

public class Certificate
{
    public int Id { get; set; }
    public int StudentProfileId { get; set; }
    public int EnrollmentId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public string PdfPath { get; set; } = string.Empty;

    public StudentProfile Student { get; set; } = null!;
    public Enrollment Enrollment { get; set; } = null!;
}
