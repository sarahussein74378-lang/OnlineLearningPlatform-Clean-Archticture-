using MediatR;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using System.Linq;

namespace OnlineLearningPlatform.Application.Features.Quizzes.Commands.SubmitQuiz;

public class SubmitQuizCommandHandler : IRequestHandler<SubmitQuizCommand, QuizResultDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public SubmitQuizCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<QuizResultDto> Handle(SubmitQuizCommand request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == request.StudentUserId);

        var quiz = await _unitOfWork.Quizzes.GetQuizWithQuestionsAndAnswersAsync(request.Dto.QuizId);

        if (quiz == null || student == null)
            throw new InvalidOperationException("Quiz or student not found.");

        var submissionAnswers = new List<QuizSubmissionAnswer>();
        int score = 0;
        int totalPoints = quiz.Questions.Sum(q => q.Points);
        var questionResults = new List<QuestionResultDto>();

        foreach (var submittedAnswer in request.Dto.Answers)
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
            QuizId = request.Dto.QuizId,
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
