using MediatR;

namespace OnlineLearningPlatform.Application.Features.Answers.Commands.DeleteAnswer;

public record DeleteAnswerCommand(int Id) : IRequest<bool>;
