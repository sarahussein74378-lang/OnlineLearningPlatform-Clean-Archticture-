using MediatR;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using System.Collections.Generic;

namespace OnlineLearningPlatform.Application.Features.Quizzes.Commands.SubmitQuiz;

public record SubmitQuizCommand(SubmitQuizDto Dto, string StudentUserId) : IRequest<QuizResultDto>;
