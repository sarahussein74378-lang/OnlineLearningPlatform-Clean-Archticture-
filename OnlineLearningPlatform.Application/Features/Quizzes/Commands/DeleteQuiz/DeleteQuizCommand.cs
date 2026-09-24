using MediatR;

namespace OnlineLearningPlatform.Application.Features.Quizzes.Commands.DeleteQuiz;

public record DeleteQuizCommand(int Id) : IRequest<bool>;
