using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.BLL.DTOs.Quiz;
using OnlineLearningPlatform.BLL.Services.Interfaces;
using OnlineLearningPlatform.DAL.Entities;
using OnlineLearningPlatform.DAL.UnitOfWork;

namespace OnlineLearningPlatform.BLL.Services.Implementations;

public class QuizService : IQuizService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QuizService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<QuizDto?> GetByIdAsync(int id)
    {
        var quiz = await _unitOfWork.Quizzes.Query()
            .Include(q => q.Questions).ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id);

        return quiz == null ? null : _mapper.Map<QuizDto>(quiz);
    }

    public async Task<IEnumerable<QuizDto>> GetByCourseAsync(int courseId)
    {
        var quizzes = await _unitOfWork.Quizzes.Query()
            .Include(q => q.Questions).ThenInclude(q => q.Answers)
            .Where(q => q.CourseId == courseId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<QuizDto>>(quizzes);
    }

    public async Task<QuizDto> CreateAsync(CreateQuizDto dto)
    {
        var quiz = _mapper.Map<Quiz>(dto);
        await _unitOfWork.Quizzes.AddAsync(quiz);
        await _unitOfWork.SaveChangesAsync();
        return (await GetByIdAsync(quiz.Id))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var quiz = await _unitOfWork.Quizzes.GetByIdAsync(id);
        if (quiz == null) return false;
        _unitOfWork.Quizzes.Delete(quiz);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<QuizResultDto> SubmitAsync(SubmitQuizDto dto, string studentUserId)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == studentUserId);

        var quiz = await _unitOfWork.Quizzes.Query()
            .Include(q => q.Questions).ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == dto.QuizId);

        if (quiz == null || student == null)
            throw new InvalidOperationException("Quiz or student not found.");

        var submissionAnswers = new List<QuizSubmissionAnswer>();
        int score = 0;
        int totalPoints = quiz.Questions.Sum(q => q.Points);
        var questionResults = new List<QuestionResultDto>();

        foreach (var submittedAnswer in dto.Answers)
        {
            var question = quiz.Questions.FirstOrDefault(q => q.Id == submittedAnswer.QuestionId);
            if (question == null) continue;

            var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == submittedAnswer.SelectedAnswerId);
            var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
            bool isCorrect = selectedAnswer?.IsCorrect ?? false;

            if (isCorrect) score += question.Points;

            submissionAnswers.Add(new QuizSubmissionAnswer
            {
                QuestionId = submittedAnswer.QuestionId,
                SelectedAnswerId = submittedAnswer.SelectedAnswerId,
                IsCorrect = isCorrect
            });

            questionResults.Add(new QuestionResultDto
            {
                QuestionId = question.Id,
                QuestionText = question.Text,
                SelectedAnswerId = submittedAnswer.SelectedAnswerId,
                CorrectAnswerId = correctAnswer?.Id ?? 0,
                IsCorrect = isCorrect
            });
        }

        var percentage = totalPoints == 0 ? 0 : Math.Round((double)score / totalPoints * 100, 1);
        var submission = new QuizSubmission
        {
            QuizId = dto.QuizId,
            StudentProfileId = student.Id,
            Score = score,
            TotalPoints = totalPoints,
            IsPassed = percentage >= quiz.PassingScore,
            SubmissionAnswers = submissionAnswers
        };

        await _unitOfWork.QuizSubmissions.AddAsync(submission);
        await _unitOfWork.SaveChangesAsync();

        return new QuizResultDto
        {
            QuizId = quiz.Id,
            QuizTitle = quiz.Title,
            Score = score,
            TotalPoints = totalPoints,
            PassingScore = quiz.PassingScore,
            IsPassed = submission.IsPassed,
            Percentage = percentage,
            QuestionResults = questionResults
        };
    }
}
